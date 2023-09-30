// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMethodMockWithTypeParameters.cs">
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

    public class PropertyBasedMethodMockWithTypeParameters : IMemberMock
    {
        public IMethodSymbol Symbol { get; }
        public string MemberMockName { get; }
        public ITypeParameterSubstitutions Substitutions { get; }
        public string MockProviderName { get; }

        public PropertyBasedMethodMockWithTypeParameters(IMethodSymbol symbol, string mockMemberName, ITypeParameterSubstitutions substitutions, string mockProviderName)
        {
            Symbol = symbol;
            MemberMockName = mockMemberName;
            Substitutions = substitutions;
            MockProviderName = mockProviderName;
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
        {
            return new SyntaxAdder(this, typesForSymbols);
        }

        public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
        {
            ctx.AppendLine(
                $"private readonly global::Mocklis.Core.TypedMockProvider {MockProviderName} = new global::Mocklis.Core.TypedMockProvider();");
            ctx.AppendSeparator();



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

            string mockPropertyType;

            if (returnValuesString == null)
            {
                mockPropertyType = parametersString == null
                    ? "global::Mocklis.Core.ActionMethodMock"
                    : $"global::Mocklis.Core.ActionMethodMock<{parametersString}>";
            }
            else
            {
                mockPropertyType = parametersString == null
                    ? $"global::Mocklis.Core.FuncMethodMock<{returnValuesString}>"
                    : $"global::Mocklis.Core.FuncMethodMock<{parametersString}, {returnValuesString}>";
            }

            var typeParameterList = string.Join(", ", Symbol.TypeParameters.Select(t => Substitutions.FindSubstitution(t.Name)));

            var constraints = ctx.BuildConstraintClauses(Symbol.TypeParameters, Substitutions, false);

            ctx.AppendLine($"public {mockPropertyType} {MemberMockName}<{typeParameterList}>(){constraints}");
            ctx.AppendLine("{");
            ctx.IncreaseIndent();

            
            //private ImplicitArrayCreationExpressionSyntax TypesOfTypeParameters()
            //{
            //    return F.ImplicitArrayCreationExpression(F.InitializerExpression(SyntaxKind.ArrayInitializerExpression,
            //        F.SeparatedList<ExpressionSyntax>(Mock.Symbol.TypeParameters.Select(typeParameter =>
            //            F.TypeOfExpression(F.IdentifierName(Mock.Substitutions.FindSubstitution(typeParameter.Name)))))));
            //}
            var typesOfTypeParameters = string.Join(", ", Symbol.TypeParameters.Select(t => $"typeof({Substitutions.FindSubstitution(t.Name)})"));

            //  var keyCreation = F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(F.SingletonSeparatedList(F
            //      .VariableDeclarator(F.Identifier("key")).WithInitializer(F.EqualsValueClause(TypesOfTypeParameters())))));

            ctx.AppendLine($"var key = new[] {{ {typesOfTypeParameters} }};");

            ctx.AppendLine($"return ({mockPropertyType}){MemberMockName}.GetOrAdd(key, {ctx.TypedMockCreation(mockPropertyType, MemberMockName, interfaceSymbol.Name, Symbol.Name)});");

            ctx.DecreaseIndent();
            ctx.AppendLine("}");
            ctx.AppendSeparator();
        //private TypeParameterListSyntax TypeParameterList()
            //{
            //    return F.TypeParameterList(F.SeparatedList(Mock.Symbol.TypeParameters.Select(typeParameter =>
            //        F.TypeParameter(Mock.Substitutions.FindSubstitution(typeParameter.Name)))));
            //}

            //private TypeArgumentListSyntax TypeArgumentList()
            //{
            //    return F.TypeArgumentList(
            //        F.SeparatedList<TypeSyntax>(Mock.Symbol.TypeParameters.Select(typeParameter =>
            //            F.IdentifierName(Mock.Substitutions.FindSubstitution(typeParameter.Name)))));
            //}


              
              //  if (constraints.Any())
              //  {
              //      m = m.AddConstraintClauses(constraints);
              //  }

              //  return m;

            var baseReturnType = returnValuesString == null
                ? "void"
                : ctx.ParseTypeName(Symbol.ReturnType, Symbol.ReturnTypeIsNullableOrOblivious(), Substitutions);


            var returnType = baseReturnType;

            if (Symbol.ReturnsByRef)
            {
                returnType = "ref " + returnType;
            }
            else if (Symbol.ReturnsByRefReadonly)
            {
                returnType = "ref readonly " + returnType;
            }

            //var mockedMethod = F.MethodDeclaration(returnType, Mock.Symbol.Name)
            //    .WithParameterList(Mock.Symbol.Parameters.AsParameterList(typesForSymbols))
            //    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

            var classConstraints = ctx.BuildConstraintClauses(Symbol.TypeParameters, Substitutions, true);
            var mockedMethod = $"{returnType} {ctx.ParseTypeName(interfaceSymbol, false, ITypeParameterSubstitutions.Empty)}.{Symbol.Name}<{typeParameterList}>({ctx.BuildParameterList(Symbol.Parameters)}){classConstraints}";

            //var memberMockInstance = MemberMockName; // ExplicitInterfaceMemberMemberMockInstance();

            // ExpressionSyntax invocation = F.InvocationExpression(
            //         F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            //             memberMockInstance, F.IdentifierName("Call")))
            //     .WithExpressionsAsArgumentList(ParametersType.BuildArgumentListWithOriginalNames());

            string invocation = $"{MemberMockName}<{typeParameterList}>().Call({parametersType.BuildArgumentListAsString()})";

            // look at the return parameters. If we don't have any we can just make the call.
            // if we only have one and that's the return value, we can just return it.
            if (returnValuesType.Count == 0 ||
                returnValuesType.Count == 1 && returnValuesType[0].IsReturnValue)
            {
                if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
                {
                    invocation = $"ref global::Mocklis.Core.ByRef<{baseReturnType}>.Wrap({invocation})";
                }

                //     mockedMethod = mockedMethod.WithExpressionBody(F.ArrowExpressionClause(invocation))
                //         .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
                mockedMethod += $" => {invocation};";

                ctx.AppendLine(mockedMethod);
            }
            // if we only have one and that's not a return value, we can just assign it to the out or ref parameter it corresponds to.
            else if (returnValuesType.Count == 1)
            {
                ctx.AppendLine(mockedMethod);
                ctx.AppendLine("{");
                ctx.IncreaseIndent();
                //     mockedMethod = mockedMethod.WithBody(F.Block(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                //         F.IdentifierName(ReturnValuesType[0].OriginalName), invocation))));
                ctx.AppendLine($"{returnValuesType[0].OriginalName} = {invocation};");
                ctx.DecreaseIndent();
                ctx.AppendLine("}");

            }
            else
            {
                ctx.AppendLine(mockedMethod);
                ctx.AppendLine("{");
                ctx.IncreaseIndent();
                // if we have more than one, put it in a temporary variable. (consider name clashes with method parameter names)
                var x = new Uniquifier(Symbol.Parameters.Select(m => m.Name));
                string tmp = x.GetUniqueName("tmp");

                //         F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(
                //             F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(tmp)).WithInitializer(F.EqualsValueClause(invocation)))))
                ctx.AppendLine($"var {tmp} = {invocation};");

                // then for any out or ref parameters, set their values from the temporary variable.
                foreach (var rv in returnValuesType.Where(a => !a.IsReturnValue))
                {
                    // statements.Add(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    //     F.IdentifierName(rv.OriginalName),
                    //     F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp),
                    //         F.IdentifierName(rv.TupleSafeName)))));
                    ctx.AppendLine($"{rv.OriginalName} = {tmp}.{rv.TupleSafeName};");
                }

                // finally, if there is a 'proper' return type, return the corresponding value from the temporary variable.
                // We cheat a little here - there will never be more than one of these guys so we use a foreach.
                foreach (var rv in returnValuesType.Where(a => a.IsReturnValue))
                {
                    if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
                    {
                        ctx.AppendLine($"return ref global::Mocklis.Core.ByRef<{baseReturnType}>.Wrap({tmp}.{rv.TupleSafeName});");
                    }
                    else
                    {
                        ctx.AppendLine($"return {tmp}.{rv.TupleSafeName};");
                    }
                }

                ctx.DecreaseIndent();
                ctx.AppendLine("}");
            }
            // string mockPropertyType, string memberMockName, string className, string interfaceName, string symbolName
            // constructorStatements.Add(typesForSymbols.InitialisationStatement(MockMemberType, Mock.MemberMockName, className, interfaceName, Mock.Symbol.Name, mockSettings.Strict, mockSettings.VeryStrict));

            // ctx.AddConstructorStatement(mockPropertyType, MemberMockName, interfaceSymbol.Name, Symbol.Name);
        }

        private class SyntaxAdder : ISyntaxAdder
        {
            private PropertyBasedMethodMockWithTypeParameters Mock { get; }

            private SingleTypeOrValueTuple ParametersType { get; }
            private SingleTypeOrValueTuple ReturnValuesType { get; }

            private TypeSyntax MockMemberType { get; }

            public SyntaxAdder(PropertyBasedMethodMockWithTypeParameters mock, MocklisTypesForSymbols typesForSymbols)
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

            public void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className, string interfaceName)
            {
                declarationList.Add(TypedMockProviderField(typesForSymbols));
                declarationList.Add(MockProviderMethod(typesForSymbols, className, interfaceName, mockSettings));
                declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax));
            }

            public void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
            }

            private MemberDeclarationSyntax TypedMockProviderField(MocklisTypesForSymbols typesForSymbols)
            {
                return F.FieldDeclaration(F.VariableDeclaration(typesForSymbols.TypedMockProvider()).WithVariables(
                    F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(Mock.MockProviderName))
                        .WithInitializer(F.EqualsValueClause(F.ObjectCreationExpression(typesForSymbols.TypedMockProvider())
                            .WithArgumentList(F.ArgumentList())))))
                ).WithModifiers(F.TokenList(F.Token(SyntaxKind.PrivateKeyword), F.Token(SyntaxKind.ReadOnlyKeyword)));
            }

            private MemberDeclarationSyntax MockProviderMethod(MocklisTypesForSymbols typesForSymbols, string className, string interfaceName, MockSettings mockSettings)
            {
                var m = F.MethodDeclaration(MockMemberType, F.Identifier(Mock.MemberMockName)).WithTypeParameterList(TypeParameterList());

                m = m.WithModifiers(F.TokenList(F.Token(SyntaxKind.PublicKeyword)));

                var keyCreation = F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(F.SingletonSeparatedList(F
                    .VariableDeclarator(F.Identifier("key")).WithInitializer(F.EqualsValueClause(TypesOfTypeParameters())))));

                var mockCreation = F.SimpleLambdaExpression(F.Parameter(F.Identifier("keyString")), F.ObjectCreationExpression(MockMemberType)
                    .WithExpressionsAsArgumentList(
                        F.ThisExpression(),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(className)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(interfaceName)),
                        F.BinaryExpression(SyntaxKind.AddExpression, F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Mock.Symbol.Name)),
                            F.IdentifierName("keyString")),
                        F.BinaryExpression(SyntaxKind.AddExpression,
                            F.BinaryExpression(SyntaxKind.AddExpression,
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Mock.MemberMockName)), F.IdentifierName("keyString")),
                            F.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                F.Literal("()"))),
                        typesForSymbols.StrictnessExpression(mockSettings.Strict, mockSettings.VeryStrict)
                    ));

                var returnStatement = F.ReturnStatement(F.CastExpression(MockMemberType, F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(Mock.MockProviderName),
                            F.IdentifierName("GetOrAdd")))
                    .WithArgumentList(
                        F.ArgumentList(F.SeparatedList(new[] { F.Argument(F.IdentifierName("key")), F.Argument(mockCreation) })))));

                m = m.WithBody(F.Block(keyCreation, returnStatement));

                var constraints = typesForSymbols.AsConstraintClauses(Mock.Symbol.TypeParameters, Mock.Substitutions.FindSubstitution);

                if (constraints.Any())
                {
                    m = m.AddConstraintClauses(constraints);
                }

                return m;
            }

            private ImplicitArrayCreationExpressionSyntax TypesOfTypeParameters()
            {
                return F.ImplicitArrayCreationExpression(F.InitializerExpression(SyntaxKind.ArrayInitializerExpression,
                    F.SeparatedList<ExpressionSyntax>(Mock.Symbol.TypeParameters.Select(typeParameter =>
                        F.TypeOfExpression(F.IdentifierName(Mock.Substitutions.FindSubstitution(typeParameter.Name)))))));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax)
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

            private MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnType, NameSyntax interfaceNameSyntax)
            {

                var m = F.MethodDeclaration(returnType, Mock.Symbol.Name)
                    .WithParameterList(Mock.Symbol.Parameters.AsParameterList(typesForSymbols))
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax))
                    .WithTypeParameterList(TypeParameterList());;

                var constraints = typesForSymbols.AsClassConstraintClausesForReferenceTypes(Mock.Symbol.TypeParameters, Mock.Substitutions.FindSubstitution);
                if (constraints.Any())
                {
                    m = m.AddConstraintClauses(constraints);
                }

                return m;
            }

            private ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
            {
                return F.InvocationExpression(F.GenericName(Mock.MemberMockName).WithTypeArgumentList(TypeArgumentList()))
                    .WithArgumentList(F.ArgumentList());
            }

            private TypeParameterListSyntax TypeParameterList()
            {
                return F.TypeParameterList(F.SeparatedList(Mock.Symbol.TypeParameters.Select(typeParameter =>
                    F.TypeParameter(Mock.Substitutions.FindSubstitution(typeParameter.Name)))));
            }

            private TypeArgumentListSyntax TypeArgumentList()
            {
                return F.TypeArgumentList(
                    F.SeparatedList<TypeSyntax>(Mock.Symbol.TypeParameters.Select(typeParameter =>
                        F.IdentifierName(Mock.Substitutions.FindSubstitution(typeParameter.Name)))));
            }
        }
    }
}
