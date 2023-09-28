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
    using Mocklis.MockGenerator.CodeGeneration;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class PropertyBasedMethodMock : IMemberMock
    {
        public IMethodSymbol Symbol { get; }
        public string MemberMockName { get; }
        public ITypeParameterSubstitutions Substitutions { get; }

        protected PropertyBasedMethodMock(IMethodSymbol symbol, string mockMemberName, ITypeParameterSubstitutions substitutions)
        {
            Symbol = symbol;
            MemberMockName = mockMemberName;
            Substitutions = substitutions;
        }

        public PropertyBasedMethodMock(IMethodSymbol symbol, string mockMemberName) : this(symbol, mockMemberName, ITypeParameterSubstitutions.Empty)
        {
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
        {
            return CreateSyntaxAdder(typesForSymbols);
        }

        public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
        {
            ctx.AppendLine("// Adding line for Property Based Method Mock");
            var parametersBuilder = new SingleTypeOrValueTupleBuilder();
            var returnValuesBuilder = new SingleTypeOrValueTupleBuilder();

            if (!Symbol.ReturnsVoid)
            {
                returnValuesBuilder.AddReturnValue(Symbol.ReturnType, Symbol.ReturnTypeIsNullableOrOblivious());
            }

            foreach (var parameter in Symbol.Parameters)
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

            var parametersType = parametersBuilder.Build();
            var returnValuesType = returnValuesBuilder.Build();

            var parametersString = ctx.BuildTupleType(parametersType, Substitutions);
            var returnValuesString = ctx.BuildTupleType(returnValuesType, Substitutions);

            string mockType;

            if (returnValuesString == null)
            {
                mockType = parametersString == null
                    ? "global::Mocklis.Core.ActionMethodMock"
                    : $"global::Mocklis.Core.ActionMethodMock<{parametersString}>";
            }
            else
            {
                mockType = parametersString == null
                    ? $"global::Mocklis.Core.FuncMethodMock<{returnValuesString}>"
                    : $"global::Mocklis.Core.FuncMethodMock<{parametersString},{returnValuesString}>";
            }
            ctx.AppendLine();
            ctx.AppendLine($"public {mockType} {MemberMockName} {{ get; }}");
            ctx.AppendLine();
           

            var baseReturnType = returnValuesString == null ? "void" : ctx.ParseTypeName(Symbol.ReturnType, Symbol.ReturnTypeIsNullableOrOblivious(), Substitutions);


            var returnType = baseReturnType;

            if (Symbol.ReturnsByRef)
            {
                returnType = "ref " + returnType;
            }
            else if (Symbol.ReturnsByRefReadonly)
            {
                returnType = "ref readonly " + returnType;
            }

            var method = $"{returnType} {ctx.ParseTypeName(interfaceSymbol, false, ITypeParameterSubstitutions.Empty)}.{Symbol.Name}({ctx.BuildParameterList(Symbol.Parameters)})";

            ctx.AppendLine(method);
            //var mockedMethod = F.MethodDeclaration(returnType, Mock.Symbol.Name)
            //    .WithParameterList(Mock.Symbol.Parameters.AsParameterList(typesForSymbols))
            //    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));
        

           //var memberMockInstance = MemberMockName; // ExplicitInterfaceMemberMemberMockInstance();

           // ExpressionSyntax invocation = F.InvocationExpression(
           //         F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
           //             memberMockInstance, F.IdentifierName("Call")))
           //     .WithExpressionsAsArgumentList(ParametersType.BuildArgumentListWithOriginalNames());

           // // look at the return parameters. If we don't have any we can just make the call.
           // // if we only have one and that's the return value, we can just return it.
           // if (ReturnValuesType.Count == 0 ||
           //     ReturnValuesType.Count == 1 && ReturnValuesType[0].IsReturnValue)
           // {
           //     if (Mock.Symbol.ReturnsByRef || Mock.Symbol.ReturnsByRefReadonly)
           //     {
           //         invocation = typesForSymbols.WrapByRef(invocation, baseReturnType);
           //     }

           //     mockedMethod = mockedMethod.WithExpressionBody(F.ArrowExpressionClause(invocation))
           //         .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
           // }
           // // if we only have one and that's not a return value, we can just assign it to the out or ref parameter it corresponds to.
           // else if (ReturnValuesType.Count == 1)
           // {
           //     mockedMethod = mockedMethod.WithBody(F.Block(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
           //         F.IdentifierName(ReturnValuesType[0].OriginalName), invocation))));
           // }
           // else
           // {
           //     // if we have more than one, put it in a temporary variable. (consider name clashes with method parameter names)
           //     var x = new Uniquifier(Mock.Symbol.Parameters.Select(m => m.Name));
           //     string tmp = x.GetUniqueName("tmp");

           //     var statements = new List<StatementSyntax>
           //     {
           //         F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(
           //             F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(tmp)).WithInitializer(F.EqualsValueClause(invocation)))))
           //     };

           //     // then for any out or ref parameters, set their values from the temporary variable.
           //     foreach (var rv in ReturnValuesType.Where(a => !a.IsReturnValue))
           //     {
           //         statements.Add(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
           //             F.IdentifierName(rv.OriginalName),
           //             F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp),
           //                 F.IdentifierName(rv.TupleSafeName)))));
           //     }

           //     // finally, if there is a 'proper' return type, return the corresponding value from the temporary variable.
           //     foreach (var rv in ReturnValuesType.Where(a => a.IsReturnValue))
           //     {
           //         ExpressionSyntax memberAccess = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp),
           //             F.IdentifierName(rv.TupleSafeName));

           //         if (Mock.Symbol.ReturnsByRef || Mock.Symbol.ReturnsByRefReadonly)
           //         {
           //             memberAccess = typesForSymbols.WrapByRef(memberAccess, baseReturnType);
           //         }

           //         statements.Add(F.ReturnStatement(memberAccess));
           //     }

           //     mockedMethod = mockedMethod.WithBody(F.Block(statements));
           // }

           // return mockedMethod;



        }

        protected virtual ISyntaxAdder CreateSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
        {
            return new SyntaxAdder<PropertyBasedMethodMock>(this, typesForSymbols);
        }

        protected class SyntaxAdder<TMock> : ISyntaxAdder where TMock : PropertyBasedMethodMock
        {
            protected TMock Mock { get; }

            protected SingleTypeOrValueTuple ParametersType { get; }
            protected SingleTypeOrValueTuple ReturnValuesType { get; }

            protected TypeSyntax MockMemberType { get; }

            public SyntaxAdder(TMock mock, MocklisTypesForSymbols typesForSymbols)
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

                var parameterTypeSyntax = ParametersType.BuildTypeSyntax(typesForSymbols, mock.Substitutions.FindSubstitution);

                var returnValueTypeSyntax = ReturnValuesType.BuildTypeSyntax(typesForSymbols, mock.Substitutions.FindSubstitution);

                if (returnValueTypeSyntax == null)
                {
                    MockMemberType = parameterTypeSyntax == null
                        ? typesForSymbols.ActionMethodMock()
                        : typesForSymbols.ActionMethodMock(parameterTypeSyntax, mock.Substitutions.FindSubstitution);
                }
                else
                {
                    MockMemberType = parameterTypeSyntax == null
                        ? typesForSymbols.FuncMethodMock(returnValueTypeSyntax, mock.Substitutions.FindSubstitution)
                        : typesForSymbols.FuncMethodMock(parameterTypeSyntax, returnValueTypeSyntax, mock.Substitutions.FindSubstitution);
                }

            }

            public virtual void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettingns,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
                string interfaceName)
            {
                declarationList.Add(MockMemberType.MockProperty(Mock.MemberMockName));
                declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax));
            }

            public virtual void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
                constructorStatements.Add(typesForSymbols.InitialisationStatement(MockMemberType, Mock.MemberMockName, className, interfaceName, Mock.Symbol.Name, mockSettings.Strict, mockSettings.VeryStrict));
            }

            protected MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax)
            {
                var baseReturnType = Mock.Symbol.ReturnsVoid
                    ? F.PredefinedType(F.Token(SyntaxKind.VoidKeyword))
                    : typesForSymbols.ParseTypeName(Mock.Symbol.ReturnType, Mock.Symbol.ReturnTypeIsNullableOrOblivious(), Mock.Substitutions.FindSubstitution);
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
                    ReturnValuesType.Count == 1 && ReturnValuesType[0].IsReturnValue)
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

            protected virtual MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnType, NameSyntax interfaceNameSyntax)
            {
                return F.MethodDeclaration(returnType, Mock.Symbol.Name)
                    .WithParameterList(Mock.Symbol.Parameters.AsParameterList(typesForSymbols))
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));
            }

            protected virtual ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
            {
                return F.IdentifierName(Mock.MemberMockName);
            }
        }
    }
}
