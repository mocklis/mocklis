// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMethodMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
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

    public class PropertyBasedMethodMock : PropertyBasedMock<IMethodSymbol>, IMemberMock
    {
        public PropertyBasedMethodMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IMethodSymbol symbol, string mockMemberName) : base(classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return CreateSyntaxAdder(typesForSymbols, strict, veryStrict);
        }

        protected virtual ISyntaxAdder CreateSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return new SyntaxAdder<PropertyBasedMethodMock>(this, typesForSymbols, strict, veryStrict);
        }

        protected class SyntaxAdder<TMock> : ISyntaxAdder where TMock : PropertyBasedMethodMock
        {
            protected TMock Mock { get; }
            protected MocklisTypesForSymbols TypesForSymbols { get; }
            protected bool Strict { get; }
            protected bool VeryStrict { get; }

            protected SingleTypeOrValueTuple ParametersType { get; }
            protected SingleTypeOrValueTuple ReturnValuesType { get; }

            protected TypeSyntax MockMemberType { get; }

            protected Substitutions Substitutions { get; }

            public SyntaxAdder(TMock mock, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
            {
                Mock = mock;
                TypesForSymbols = typesForSymbols;
                Substitutions = new Substitutions(Mock.ClassSymbol, Mock.Symbol);
                Strict = strict;
                VeryStrict = veryStrict;

                var parametersBuilder = new SingleTypeOrValueTupleBuilder(TypesForSymbols);
                var returnValuesBuilder = new SingleTypeOrValueTupleBuilder(TypesForSymbols);

                if (!Mock.Symbol.ReturnsVoid)
                {
                    returnValuesBuilder.AddReturnValue(Mock.Symbol.ReturnType, Mock.Symbol.ReturnTypeIsNullableOrOblivious(), Substitutions.FindTypeParameterName);
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
                        : typesForSymbols.ActionMethodMock(parameterTypeSyntax, Substitutions.FindTypeParameterName);
                }
                else
                {
                    MockMemberType = parameterTypeSyntax == null
                        ? typesForSymbols.FuncMethodMock(returnValueTypeSyntax, Substitutions.FindTypeParameterName)
                        : typesForSymbols.FuncMethodMock(parameterTypeSyntax, returnValueTypeSyntax, Substitutions.FindTypeParameterName);
                }

            }

            public virtual void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
            {
                declarationList.Add(Mock.MockProperty(MockMemberType));
                declarationList.Add(ExplicitInterfaceMember());
            }

            public virtual void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
                constructorStatements.Add(Mock.InitialisationStatement(MockMemberType, TypesForSymbols, Strict, VeryStrict));
            }

            protected MemberDeclarationSyntax ExplicitInterfaceMember()
            {
                var baseReturnType = Mock.Symbol.ReturnsVoid
                    ? F.PredefinedType(F.Token(SyntaxKind.VoidKeyword))
                    : TypesForSymbols.ParseTypeName(Mock.Symbol.ReturnType, Mock.Symbol.ReturnTypeIsNullableOrOblivious(), Substitutions.FindTypeParameterName);
                var returnType = baseReturnType;

                if (Mock.Symbol.ReturnsByRef)
                {
                    returnType = F.RefType(returnType);
                }
                else if (Mock.Symbol.ReturnsByRefReadonly)
                {
                    returnType = F.RefType(returnType).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                }

                var mockedMethod = ExplicitInterfaceMemberMethodDeclaration(returnType);

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

            protected virtual MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(TypeSyntax returnType)
            {
                return F.MethodDeclaration(returnType, Mock.Symbol.Name)
                    .WithParameterList(Mock.Symbol.Parameters.AsParameterList(TypesForSymbols))
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(Mock.InterfaceSymbol)));
            }

            protected virtual ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
            {
                return F.IdentifierName(Mock.MemberMockName);
            }
        }
    }
}
