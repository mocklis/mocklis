// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMethodMock.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class PropertyBasedMethodMock : PropertyBasedMock<IMethodSymbol>, IMemberMock
    {
        private ParameterOrReturnValue[] MockParameters { get; }
        private ParameterOrReturnValue[] MockReturnValues { get; }
        private ParameterOrReturnValue[] MethodParameters { get; }
        protected TypeSyntax MockMemberType { get; }
        protected TypeParameterNameSubstitutions TypeParameterNameSubstitutions { get; }

        public PropertyBasedMethodMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IMethodSymbol symbol,
            string mockMemberName) : base(typesForSymbols,
            classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            TypeParameterNameSubstitutions = new TypeParameterNameSubstitutions(classSymbol, symbol);

            var parameterOrReturnValueList = new List<ParameterOrReturnValue>();

            var uniquifier = new Uniquifier();

            if (!symbol.ReturnsVoid)
            {
                parameterOrReturnValueList.Add(new ParameterOrReturnValue(ParameterOrReturnValueKind.ReturnValue, "returnValue",
                    uniquifier.GetUniqueName("returnValue"), typesForSymbols.ParseTypeName(symbol.ReturnType, TypeParameterNameSubstitutions)));
            }

            foreach (var parameter in symbol.Parameters)
            {
                if (parameter.IsThis)
                {
                    continue;
                }

                ParameterOrReturnValueKind kind;
                switch (parameter.RefKind)
                {
                    case RefKind.Ref:
                    {
                        kind = ParameterOrReturnValueKind.Ref;
                        break;
                    }
                    case RefKind.Out:
                    {
                        kind = ParameterOrReturnValueKind.Out;
                        break;
                    }
                    case RefKind.In:
                    {
                        kind = ParameterOrReturnValueKind.In;
                        break;
                    }
                    case RefKind.None:
                    {
                        kind = ParameterOrReturnValueKind.Normal;
                        break;
                    }
                    default:
                    {
                        continue;
                    }
                }

                parameterOrReturnValueList.Add(new ParameterOrReturnValue(kind, parameter.Name, uniquifier.GetUniqueName(parameter.Name),
                    typesForSymbols.ParseTypeName(parameter.Type, TypeParameterNameSubstitutions)));
            }

            MockParameters = parameterOrReturnValueList.Where(i =>
                i.Kind == ParameterOrReturnValueKind.Normal || i.Kind == ParameterOrReturnValueKind.In ||
                i.Kind == ParameterOrReturnValueKind.Ref).ToArray();

            MockReturnValues = parameterOrReturnValueList.Where(i =>
                i.Kind == ParameterOrReturnValueKind.ReturnValue || i.Kind == ParameterOrReturnValueKind.Ref ||
                i.Kind == ParameterOrReturnValueKind.Out).ToArray();

            MethodParameters = parameterOrReturnValueList.Where(i => i.Kind != ParameterOrReturnValueKind.ReturnValue).ToArray();

            var parameterTypeSyntax = ParameterOrReturnValue.BuildType(MockParameters);

            var returnValueTypeSyntax = ParameterOrReturnValue.BuildType(MockReturnValues);

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
            var baseReturnType = TypesForSymbols.ParseTypeName(Symbol.ReturnType, TypeParameterNameSubstitutions);
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
                .WithExpressionsAsArgumentList(ParameterOrReturnValue.BuildArgument(MockParameters));

            // look at the return parameters. If we don't have any we can just make the call.
            // if we only have one and that's the return value, we can just return it.
            if (MockReturnValues.Length == 0 ||
                MockReturnValues.Length == 1 && MockReturnValues[0].Kind == ParameterOrReturnValueKind.ReturnValue)
            {
                if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
                {
                    invocation = TypesForSymbols.WrapByRef(invocation, baseReturnType);
                }

                mockedMethod = mockedMethod.WithExpressionBody(F.ArrowExpressionClause(invocation))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }

            // if we only have one and that's not a return value, we can just assign it to the out or ref parameter it corresponds to.
            else if (MockReturnValues.Length == 1)
            {
                mockedMethod = mockedMethod.WithBody(F.Block(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    F.IdentifierName(MockReturnValues[0].ParameterName), invocation))));
            }

            else
            {
                // if we have more than one, put it in a temporary variable. (consider name clashes with method parameter names)

                var x = new Uniquifier(MethodParameters.Select(m => m.ParameterName));
                string tmp = x.GetUniqueName("tmp");

                var statements = new List<StatementSyntax>
                {
                    F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(
                        F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(tmp)).WithInitializer(F.EqualsValueClause(invocation)))))
                };

                // then for any out or ref parameters, set their values from the temporary variable.
                foreach (var rv in MockReturnValues.Where(a => a.Kind != ParameterOrReturnValueKind.ReturnValue))
                {
                    statements.Add(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                        F.IdentifierName(rv.ParameterName),
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp), F.IdentifierName(rv.UniqueName)))));
                }

                // finally, if there is a 'proper' return type, return the corresponding value from the temporary variable.
                foreach (var rv in MockReturnValues.Where(a => a.Kind == ParameterOrReturnValueKind.ReturnValue))
                {
                    ExpressionSyntax memberAccess = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp),
                        F.IdentifierName(rv.UniqueName));

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
                .WithParameterList(F.ParameterList(F.SeparatedList(MethodParameters.Select(p => p.AsParameterSyntax()))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));
        }

        protected virtual ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
        {
            return F.IdentifierName(MemberMockName);
        }

        protected enum ParameterOrReturnValueKind
        {
            ReturnValue,
            Normal,
            In,
            Out,
            Ref
        }

        protected class ParameterOrReturnValue
        {
            public ParameterOrReturnValueKind Kind { get; }
            public string ParameterName { get; }
            public string UniqueName { get; }
            private TypeSyntax TypeSyntax { get; }

            public ParameterOrReturnValue(ParameterOrReturnValueKind kind, string parameterName, string uniqueName, TypeSyntax typeSyntax)
            {
                Kind = kind;
                ParameterName = parameterName;
                UniqueName = uniqueName;
                TypeSyntax = typeSyntax;
            }

            public static TypeSyntax BuildType(IEnumerable<ParameterOrReturnValue> items)
            {
                var itemsArray = items.ToArray();
                switch (itemsArray.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return itemsArray[0].TypeSyntax;
                    default:
                        return F.TupleType(F.SeparatedList(itemsArray.Select(a => F.TupleElement(a.TypeSyntax, F.Identifier(a.UniqueName)))));
                }
            }

            public static ExpressionSyntax BuildArgument(IEnumerable<ParameterOrReturnValue> items)
            {
                var itemsArray = items.ToArray();
                switch (itemsArray.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return F.IdentifierName(itemsArray[0].ParameterName);
                    default:
                        return F.TupleExpression(F.SeparatedList(itemsArray.Select(a => F.Argument(F.IdentifierName(a.ParameterName)))));
                }
            }

            public ParameterSyntax AsParameterSyntax()
            {
                var result = F.Parameter(F.Identifier(ParameterName)).WithType(TypeSyntax);
                switch (Kind)
                {
                    case ParameterOrReturnValueKind.In:
                    {
                        return result.WithModifiers(F.TokenList(F.Token(SyntaxKind.InKeyword)));
                    }
                    case ParameterOrReturnValueKind.Out:
                    {
                        return result.WithModifiers(F.TokenList(F.Token(SyntaxKind.OutKeyword)));
                    }
                    case ParameterOrReturnValueKind.Ref:
                    {
                        return result.WithModifiers(F.TokenList(F.Token(SyntaxKind.RefKeyword)));
                    }
                    default:
                        return result;
                }
            }
        }
    }
}
