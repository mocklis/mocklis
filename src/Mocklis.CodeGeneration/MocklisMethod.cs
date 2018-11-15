// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisMethod.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class MocklisMethod : MocklisMember<IMethodSymbol>
    {
        private (string uniqueName, ParameterOrReturnValue item)[] MockParameters { get; }
        private (string uniqueName, ParameterOrReturnValue item)[] MockReturnValues { get; }
        private (string uniqueName, ParameterOrReturnValue item)[] MethodParameters { get; }

        public MocklisMethod(MocklisClass mocklisClass, INamedTypeSymbol interfaceSymbol, IMethodSymbol symbol) : base(mocklisClass,
            interfaceSymbol, symbol)
        {
            var parameterOrReturnValueList = new List<ParameterOrReturnValue>();

            if (!symbol.ReturnsVoid)
            {
                parameterOrReturnValueList.Add(new ParameterOrReturnValue(ParameterOrReturnValueKind.ReturnValue, "returnValue",
                    mocklisClass.ParseTypeName(symbol.ReturnType)));
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

                parameterOrReturnValueList.Add(new ParameterOrReturnValue(kind, parameter.Name,
                    mocklisClass.ParseTypeName(parameter.Type)));
            }

            var parameters = Uniquifier.GetUniqueNames(parameterOrReturnValueList).ToArray();

            MockParameters = parameters.Where(i =>
                i.item.Kind == ParameterOrReturnValueKind.Normal || i.item.Kind == ParameterOrReturnValueKind.In ||
                i.item.Kind == ParameterOrReturnValueKind.Ref).ToArray();

            MockReturnValues = parameters.Where(i =>
                i.item.Kind == ParameterOrReturnValueKind.ReturnValue || i.item.Kind == ParameterOrReturnValueKind.Ref ||
                i.item.Kind == ParameterOrReturnValueKind.Out).ToArray();

            MethodParameters = parameters.Where(i => i.item.Kind != ParameterOrReturnValueKind.ReturnValue).ToArray();

            var parameterTypeSyntax = ParameterOrReturnValue.BuildType(MockParameters);

            var returnValueTypeSyntax = ParameterOrReturnValue.BuildType(MockReturnValues);

            if (returnValueTypeSyntax == null)
            {
                MockPropertyType = parameterTypeSyntax == null
                    ? MocklisClass.ActionMethodMock()
                    : MocklisClass.ActionMethodMock(parameterTypeSyntax);
            }
            else
            {
                MockPropertyType = parameterTypeSyntax == null
                    ? MocklisClass.FuncMethodMock(returnValueTypeSyntax)
                    : MocklisClass.FuncMethodMock(parameterTypeSyntax, returnValueTypeSyntax);
            }

            MockPropertyInterfaceType = MocklisClass.MethodStepCallerMock(parameterTypeSyntax ?? MocklisClass.ValueTuple,
                returnValueTypeSyntax ?? MocklisClass.ValueTuple);
        }

        public override TypeSyntax MockPropertyType { get; }

        public override TypeSyntax MockPropertyInterfaceType { get; }

        public override MemberDeclarationSyntax ExplicitInterfaceMember(string mockPropertyName)
        {
            // we currently don't add ref, in or out as required.
            var mockedMethod = F.MethodDeclaration(MocklisClass.ParseTypeName(Symbol.ReturnType), Symbol.Name)
                .WithParameterList(F.ParameterList(F.SeparatedList(MethodParameters.Select(p => p.item.AsParameterSyntax()))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(InterfaceName));

            var invocation = F.InvocationExpression(
                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        F.IdentifierName(mockPropertyName), F.IdentifierName("Call")))
                .WithExpressionsAsArgumentList(ParameterOrReturnValue.BuildArgument(MockParameters.Select(a => a.item)));

            // look at the return parameters. If we don't have any we can just make the call.
            // if we only have one and that's the return value, we can just return it.
            if (MockReturnValues.Length == 0 ||
                MockReturnValues.Length == 1 && MockReturnValues[0].item.Kind == ParameterOrReturnValueKind.ReturnValue)
            {
                mockedMethod = mockedMethod.WithExpressionBody(F.ArrowExpressionClause(invocation))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }

            // if we only have one and that's not a return value, we can just assign it to the out or ref parameter it corresponds to.
            else if (MockReturnValues.Length == 1)
            {
                mockedMethod = mockedMethod.WithBody(F.Block(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    F.IdentifierName(MockReturnValues[0].item.PreferredName), invocation))));
            }

            else
            {
                // if we have more than one, put it in a temporary variable. (consider name clashes with method parameter names)

                var x = new Uniquifier(MethodParameters.Select(m => m.item.PreferredName));
                string tmp = x.GetUniqueName("tmp");

                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(
                        F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(tmp)).WithInitializer(F.EqualsValueClause(invocation)))))
                };

                // then for any out or ref parameters, set their values from the temporary variable.
                foreach (var rv in MockReturnValues.Where(a => a.item.Kind != ParameterOrReturnValueKind.ReturnValue))
                {
                    statements.Add(F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                        F.IdentifierName(rv.item.PreferredName),
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp), F.IdentifierName(rv.uniqueName)))));
                }

                // finally, if there is a 'proper' return type, return the corresponding value from the temporary variable.
                foreach (var rv in MockReturnValues.Where(a => a.item.Kind == ParameterOrReturnValueKind.ReturnValue))
                {
                    statements.Add(F.ReturnStatement(F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(tmp),
                        F.IdentifierName(rv.uniqueName))));
                }

                mockedMethod = mockedMethod.WithBody(F.Block(statements));
            }

            return mockedMethod;
        }

        private enum ParameterOrReturnValueKind
        {
            ReturnValue,
            Normal,
            In,
            Out,
            Ref
        }

        private class ParameterOrReturnValue : IHasPreferredName
        {
            public ParameterOrReturnValueKind Kind { get; }
            public string PreferredName { get; }
            public TypeSyntax TypeSyntax { get; }

            public ParameterOrReturnValue(ParameterOrReturnValueKind kind, string preferredName, TypeSyntax typeSyntax)
            {
                Kind = kind;
                PreferredName = preferredName;
                TypeSyntax = typeSyntax;
            }

            public static TypeSyntax BuildType(IEnumerable<(string uniqueName, ParameterOrReturnValue item)> items)
            {
                var itemsArray = items.ToArray();
                switch (itemsArray.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return itemsArray[0].item.TypeSyntax;
                    default:
                        return F.TupleType(F.SeparatedList(itemsArray.Select(a => F.TupleElement(a.item.TypeSyntax, F.Identifier(a.uniqueName)))));
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
                        return F.IdentifierName(itemsArray[0].PreferredName);
                    default:
                        return F.TupleExpression(F.SeparatedList(itemsArray.Select(a => F.Argument(F.IdentifierName(a.PreferredName)))));
                }
            }

            public ParameterSyntax AsParameterSyntax()
            {
                var result = F.Parameter(F.Identifier(PreferredName)).WithType(TypeSyntax);
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
