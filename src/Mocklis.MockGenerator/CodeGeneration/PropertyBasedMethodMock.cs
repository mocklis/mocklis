// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMethodMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#endregion

public sealed class PropertyBasedMethodMock : IMemberMock
{
    public IMethodSymbol Symbol { get; }
    public string MemberMockName { get; }

    public PropertyBasedMethodMock(IMethodSymbol symbol, string mockMemberName)
    {
        Symbol = symbol;
        MemberMockName = mockMemberName;
    }

    public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements,
        NameSyntax interfaceNameSyntax, string className, string interfaceName)
    {
        var syntaxAdder = new SyntaxAdder(this, typesForSymbols);
        syntaxAdder.AddMembersToClass(typesForSymbols, declarationList, interfaceNameSyntax);
        syntaxAdder.AddInitialisersToConstructor(typesForSymbols, constructorStatements, className, interfaceName);
    }

    private class SyntaxAdder
    {
        private PropertyBasedMethodMock Mock { get; }

        private SingleTypeOrValueTuple ParametersType { get; }
        private SingleTypeOrValueTuple ReturnValuesType { get; }

        private TypeSyntax MockMemberType { get; }

        public SyntaxAdder(PropertyBasedMethodMock mock, MocklisTypesForSymbols typesForSymbols)
        {
            Mock = mock;

            var parametersBuilder = new SingleTypeOrValueTupleBuilder();
            var returnValuesBuilder = new SingleTypeOrValueTupleBuilder();

            if (!Mock.Symbol.ReturnsVoid)
            {
                returnValuesBuilder.AddReturnValue(Mock.Symbol.ReturnType, Mock.Symbol.ReturnTypeIsNullableOrOblivious());
            }

            foreach (var parameter in Mock.Symbol.Parameters)
            {
                switch (parameter.RefKind)
                {
                    case RefKind.Ref:
                    {
                        parametersBuilder.AddParameter(parameter);
                        returnValuesBuilder.AddParameter(parameter);
                        break;
                    }

                    case RefKind.Out:
                    {
                        returnValuesBuilder.AddParameter(parameter);
                        break;
                    }

                    case RefKind.In:
                    {
                        parametersBuilder.AddParameter(parameter);
                        break;
                    }

                    case RefKind.None:
                    {
                        parametersBuilder.AddParameter(parameter);
                        break;
                    }
                }
            }

            ParametersType = parametersBuilder.Build();
            ReturnValuesType = returnValuesBuilder.Build();

            var parameterTypeSyntax = ParametersType.BuildTypeSyntax(typesForSymbols, null);

            var returnValueTypeSyntax = ReturnValuesType.BuildTypeSyntax(typesForSymbols, null);

            if (returnValueTypeSyntax == null)
            {
                MockMemberType = parameterTypeSyntax == null
                    ? typesForSymbols.ActionMethodMock()
                    : typesForSymbols.ActionMethodMock(parameterTypeSyntax);
            }
            else
            {
                MockMemberType = parameterTypeSyntax == null
                    ? typesForSymbols.FuncMethodMock(returnValueTypeSyntax)
                    : typesForSymbols.FuncMethodMock(parameterTypeSyntax, returnValueTypeSyntax);
            }
        }

        public void AddMembersToClass(MocklisTypesForSymbols typesForSymbols,
            IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax)
        {
            declarationList.Add(MockMemberType.MockProperty(Mock.MemberMockName));
            declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax));
        }

        public void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, List<StatementSyntax> constructorStatements,
            string className, string interfaceName)
        {
            constructorStatements.Add(typesForSymbols.InitialisationStatement(MockMemberType, Mock.MemberMockName, className, interfaceName,
                Mock.Symbol.Name));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax)
        {
            var baseReturnType = Mock.Symbol.ReturnsVoid
                ? F.PredefinedType(F.Token(SyntaxKind.VoidKeyword))
                : typesForSymbols.ParseTypeName(Mock.Symbol.ReturnType, Mock.Symbol.ReturnTypeIsNullableOrOblivious());
            var returnType = baseReturnType;

            if (Mock.Symbol.ReturnsByRef)
            {
                returnType = F.RefType(returnType);
            }
            else if (Mock.Symbol.ReturnsByRefReadonly)
            {
                returnType = F.RefType(returnType).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
            }

            var mockedMethod = ExplicitInterfaceMemberMethodDeclaration(typesForSymbols, returnType, interfaceNameSyntax);

            var memberMockInstance = ExplicitInterfaceMemberMemberMockInstance();

            ExpressionSyntax invocation = F.InvocationExpression(
                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        memberMockInstance, F.IdentifierName("Call")))
                .WithExpressionsAsArgumentList(ParametersType.BuildArgumentListWithOriginalNames());

            // look at the return parameters. If we don't have any we can just make the call.
            // if we only have one and that's the return value, we can just return it.
            if (ReturnValuesType.Count == 0 ||
                (ReturnValuesType.Count == 1 && ReturnValuesType[0].IsReturnValue))
            {
                if (Mock.Symbol.ReturnsByRef || Mock.Symbol.ReturnsByRefReadonly)
                {
                    invocation = typesForSymbols.WrapByRef(invocation, baseReturnType);
                }

                mockedMethod = mockedMethod.WithExpressionBody(F.ArrowExpressionClause(invocation))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            // if we only have one and that's not a return value, we can just assign it to the out or ref parameter it corresponds to.
            else if (ReturnValuesType.Count == 1)
            {
                mockedMethod = mockedMethod.WithBody(F.Block(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    F.IdentifierName(ReturnValuesType[0].OriginalName), invocation))));
            }
            else
            {
                // if we have more than one, put it in a temporary variable. (consider name clashes with method parameter names)
                var x = new Uniquifier(Mock.Symbol.Parameters.Select(m => m.Name));
                string tmp = x.GetUniqueName("tmp");

                var statements = new List<StatementSyntax>
                {
                    F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(
                        F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(tmp)).WithInitializer(F.EqualsValueClause(invocation)))))
                };

                // then for any out or ref parameters, set their values from the temporary variable.
                foreach (var rv in ReturnValuesType.Where(a => !a.IsReturnValue))
                {
                    statements.Add(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                        F.IdentifierName(rv.OriginalName),
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp),
                            F.IdentifierName(rv.TupleSafeName)))));
                }

                // finally, if there is a 'proper' return type, return the corresponding value from the temporary variable.
                foreach (var rv in ReturnValuesType.Where(a => a.IsReturnValue))
                {
                    ExpressionSyntax memberAccess = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp),
                        F.IdentifierName(rv.TupleSafeName));

                    if (Mock.Symbol.ReturnsByRef || Mock.Symbol.ReturnsByRefReadonly)
                    {
                        memberAccess = typesForSymbols.WrapByRef(memberAccess, baseReturnType);
                    }

                    statements.Add(F.ReturnStatement(memberAccess));
                }

                mockedMethod = mockedMethod.WithBody(F.Block(statements));
            }

            return mockedMethod;
        }

        private MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnType,
            NameSyntax interfaceNameSyntax)
        {
            return F.MethodDeclaration(returnType, Mock.Symbol.Name)
                .WithParameterList(Mock.Symbol.Parameters.AsParameterList(typesForSymbols))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));
        }

        private ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
        {
            return F.IdentifierName(Mock.MemberMockName);
        }
    }
}
