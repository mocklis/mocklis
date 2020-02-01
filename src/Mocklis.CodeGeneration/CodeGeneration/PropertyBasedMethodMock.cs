// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMethodMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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
        private SingleTypeOrValueTuple ParametersType { get; }
        private SingleTypeOrValueTuple ReturnValuesType { get; }

        protected TypeSyntax MockMemberType { get; }

        public PropertyBasedMethodMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IMethodSymbol symbol, string mockMemberName, bool strict, bool veryStrict) : base(typesForSymbols, classSymbol, interfaceSymbol, symbol,
            mockMemberName, strict, veryStrict)
        {
            var parametersBuilder = new SingleTypeOrValueTupleBuilder(TypesForSymbols);
            var returnValuesBuilder = new SingleTypeOrValueTupleBuilder(TypesForSymbols);

            if (!symbol.ReturnsVoid)
            {
                returnValuesBuilder.AddReturnValue(symbol.ReturnType, symbol.ReturnTypeIsNullableOrOblivious());
            }

            foreach (var parameter in symbol.Parameters)
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
                    ? TypesForSymbols.ActionMethodMock()
                    : TypesForSymbols.ActionMethodMock(parameterTypeSyntax);
            }
            else
            {
                MockMemberType = parameterTypeSyntax == null
                    ? TypesForSymbols.FuncMethodMock(returnValueTypeSyntax)
                    : TypesForSymbols.FuncMethodMock(parameterTypeSyntax, returnValueTypeSyntax);
            }
        }

        public virtual void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
        {
            declarationList.Add(MockProperty(MockMemberType));
            declarationList.Add(ExplicitInterfaceMember());
        }

        public virtual void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
        {
            constructorStatements.Add(InitialisationStatement(MockMemberType));
        }

        protected MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var baseReturnType = Symbol.ReturnsVoid
                ? F.PredefinedType(F.Token(SyntaxKind.VoidKeyword))
                : TypesForSymbols.ParseTypeName(Symbol.ReturnType, Symbol.ReturnTypeIsNullableOrOblivious());
            var returnType = baseReturnType;

            if (Symbol.ReturnsByRef)
            {
                returnType = F.RefType(returnType);
            }
            else if (Symbol.ReturnsByRefReadonly)
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
                if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
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
                var x = new Uniquifier(Symbol.Parameters.Select(m => m.Name));
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

                    if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
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
            return F.MethodDeclaration(returnType, Symbol.Name)
                .WithParameterList(Symbol.Parameters.AsParameterList(TypesForSymbols))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));
        }

        protected virtual ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
        {
            return F.IdentifierName(MemberMockName);
        }
    }
}
