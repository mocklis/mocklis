// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMethodMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class PropertyBasedMethodMock : IMemberMock
    {
        public IMethodSymbol Symbol { get; }
        public string MemberMockName { get; }
        public Substitutions Substitutions { get; }

        public PropertyBasedMethodMock(IMethodSymbol symbol, string mockMemberName, Substitutions substitutions)
        {
            Symbol = symbol;
            MemberMockName = mockMemberName;
            Substitutions = substitutions;
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
        {
            return CreateSyntaxAdder(typesForSymbols);
        }

        protected virtual ISyntaxAdder CreateSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
        {
            return new SyntaxAdder<PropertyBasedMethodMock>(this, typesForSymbols);
        }

        protected class SyntaxAdder<TMock> : ISyntaxAdder where TMock : PropertyBasedMethodMock
        {
            protected TMock Mock { get; }
            protected MocklisTypesForSymbols TypesForSymbols { get; }

            protected SingleTypeOrValueTuple ParametersType { get; }
            protected SingleTypeOrValueTuple ReturnValuesType { get; }

            protected TypeSyntax MockMemberType { get; }

            

            public SyntaxAdder(TMock mock, MocklisTypesForSymbols typesForSymbols)
            {
                Mock = mock;
                TypesForSymbols = typesForSymbols;

                var parametersBuilder = new SingleTypeOrValueTupleBuilder(TypesForSymbols);
                var returnValuesBuilder = new SingleTypeOrValueTupleBuilder(TypesForSymbols);

                if (!Mock.Symbol.ReturnsVoid)
                {
                    returnValuesBuilder.AddReturnValue(Mock.Symbol.ReturnType, Mock.Symbol.ReturnTypeIsNullableOrOblivious(), mock.Substitutions.FindTypeParameterName);
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

                var parameterTypeSyntax = ParametersType.BuildTypeSyntax();

                var returnValueTypeSyntax = ReturnValuesType.BuildTypeSyntax();

                if (returnValueTypeSyntax == null)
                {
                    MockMemberType = parameterTypeSyntax == null
                        ? typesForSymbols.ActionMethodMock()
                        : typesForSymbols.ActionMethodMock(parameterTypeSyntax, mock.Substitutions.FindTypeParameterName);
                }
                else
                {
                    MockMemberType = parameterTypeSyntax == null
                        ? typesForSymbols.FuncMethodMock(returnValueTypeSyntax, mock.Substitutions.FindTypeParameterName)
                        : typesForSymbols.FuncMethodMock(parameterTypeSyntax, returnValueTypeSyntax, mock.Substitutions.FindTypeParameterName);
                }

            }

            public virtual void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettingns,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
                string interfaceName)
            {
                declarationList.Add(MockMemberType.MockProperty(Mock.MemberMockName));
                declarationList.Add(ExplicitInterfaceMember(interfaceNameSyntax));
            }

            public virtual void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
                constructorStatements.Add(TypesForSymbols.InitialisationStatement(MockMemberType, Mock.MemberMockName, className, interfaceName, Mock.Symbol.Name, mockSettings.Strict, mockSettings.VeryStrict));
            }

            protected MemberDeclarationSyntax ExplicitInterfaceMember(NameSyntax interfaceNameSyntax)
            {
                var baseReturnType = Mock.Symbol.ReturnsVoid
                    ? F.PredefinedType(F.Token(SyntaxKind.VoidKeyword))
                    : TypesForSymbols.ParseTypeName(Mock.Symbol.ReturnType, Mock.Symbol.ReturnTypeIsNullableOrOblivious(), Mock.Substitutions.FindTypeParameterName);
                var returnType = baseReturnType;

                if (Mock.Symbol.ReturnsByRef)
                {
                    returnType = F.RefType(returnType);
                }
                else if (Mock.Symbol.ReturnsByRefReadonly)
                {
                    returnType = F.RefType(returnType).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                }

                var mockedMethod = ExplicitInterfaceMemberMethodDeclaration(returnType, interfaceNameSyntax);

                var memberMockInstance = ExplicitInterfaceMemberMemberMockInstance();

                ExpressionSyntax invocation = F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            memberMockInstance, F.IdentifierName("Call")))
                    .WithExpressionsAsArgumentList(ParametersType.BuildArgumentListWithOriginalNames());

                // look at the return parameters. If we don't have any we can just make the call.
                // if we only have one and that's the return value, we can just return it.
                if (ReturnValuesType.Count == 0 ||
                    ReturnValuesType.Count == 1 && ReturnValuesType[0].IsReturnValue)
                {
                    if (Mock.Symbol.ReturnsByRef || Mock.Symbol.ReturnsByRefReadonly)
                    {
                        invocation = TypesForSymbols.WrapByRef(invocation, baseReturnType);
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
                            memberAccess = TypesForSymbols.WrapByRef(memberAccess, baseReturnType);
                        }

                        statements.Add(F.ReturnStatement(memberAccess));
                    }

                    mockedMethod = mockedMethod.WithBody(F.Block(statements));
                }

                return mockedMethod;
            }

            protected virtual MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(TypeSyntax returnType, NameSyntax interfaceNameSyntax)
            {
                return F.MethodDeclaration(returnType, Mock.Symbol.Name)
                    .WithParameterList(Mock.Symbol.Parameters.AsParameterList(TypesForSymbols))
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));
            }

            protected virtual ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
            {
                return F.IdentifierName(Mock.MemberMockName);
            }
        }
    }
}
