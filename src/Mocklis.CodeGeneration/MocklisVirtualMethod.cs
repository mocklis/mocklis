// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisVirtualMethod.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class MocklisVirtualMethod : MocklisMember<IMethodSymbol>
    {
        private TypeSyntax ReturnTypeWithoutReadonly { get; }
        private TypeSyntax ReturnType { get; }

        public MocklisVirtualMethod(MocklisClass mocklisClass, INamedTypeSymbol interfaceSymbol, IMethodSymbol symbol) : base(mocklisClass,
            interfaceSymbol, symbol)
        {
            if (symbol.ReturnsByRef)
            {
                RefTypeSyntax tmp = F.RefType(mocklisClass.ParseTypeName(symbol.ReturnType));
                ReturnType = tmp;
                ReturnTypeWithoutReadonly = tmp;
            }
            else if (symbol.ReturnsByRefReadonly)
            {
                RefTypeSyntax tmp = F.RefType(mocklisClass.ParseTypeName(symbol.ReturnType));
                ReturnType = tmp.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                ReturnTypeWithoutReadonly = tmp;
            }
            else if (symbol.ReturnsVoid)
            {
                ReturnType = F.PredefinedType(F.Token(SyntaxKind.VoidKeyword));
                ReturnTypeWithoutReadonly = ReturnType;
            }
            else
            {
                ReturnType = mocklisClass.ParseTypeName(symbol.ReturnType);
                ReturnTypeWithoutReadonly = ReturnType;
            }
        }

        public override TypeSyntax MockPropertyType { get; }

        public override StatementSyntax InitialiseMockProperty(string memberMockName)
        {
            return null;
        }

        public override MemberDeclarationSyntax MockProperty(string memberMockName)
        {
            return F.MethodDeclaration(ReturnTypeWithoutReadonly, F.Identifier(memberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithParameterList(F.ParameterList(F.SeparatedList(Symbol.Parameters.Select(ConvertParameter))))
                .WithBody(
                    F.Block(F.ThrowStatement(F.ObjectCreationExpression(MocklisClass.MockMissingException)
                            .WithExpressionsAsArgumentList(
                                F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, MocklisClass.MockType,
                                    F.IdentifierName("VirtualMethod")),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(MocklisClass.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(memberMockName))
                            )
                        )
                    ));
        }

        public override MemberDeclarationSyntax ExplicitInterfaceMember(string memberMockName)
        {
            var mockedMethod = F.MethodDeclaration(ReturnType, Symbol.Name)
                .WithParameterList(F.ParameterList(F.SeparatedList(Symbol.Parameters.Select(ConvertParameter))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(InterfaceName));

            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                mockedMethod = mockedMethod
                    .WithExpressionBody(F.ArrowExpressionClause(F.RefExpression(F.InvocationExpression(F.IdentifierName(memberMockName),
                        F.ArgumentList(F.SeparatedList(Symbol.Parameters.Select(ConvertArgument)))))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                mockedMethod = mockedMethod
                    .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(memberMockName))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }

            return mockedMethod;
        }

        private ArgumentSyntax ConvertArgument(IParameterSymbol p)
        {
            var syntax = F.Argument(F.IdentifierName(p.Name));

            switch (p.RefKind)
            {
                case RefKind.Out:
                {
                    syntax = syntax.WithRefOrOutKeyword(F.Token(SyntaxKind.OutKeyword));
                    break;
                }
                case RefKind.Ref:
                {
                    syntax = syntax.WithRefOrOutKeyword(F.Token(SyntaxKind.RefKeyword));
                    break;
                }
            }

            return syntax;
        }

        private ParameterSyntax ConvertParameter(IParameterSymbol p)
        {
            var syntax = F.Parameter(F.Identifier(p.Name)).WithType(MocklisClass.ParseTypeName(p.Type));

            switch (p.RefKind)
            {
                case RefKind.In:
                {
                    syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.InKeyword)));
                    break;
                }
                case RefKind.Out:
                {
                    syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.OutKeyword)));
                    break;
                }
                case RefKind.Ref:
                {
                    syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.RefKeyword)));
                    break;
                }
            }

            return syntax;
        }
    }
}
