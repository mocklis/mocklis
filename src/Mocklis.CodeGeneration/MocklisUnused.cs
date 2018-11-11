// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisUnused.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class MocklisUnused : MocklisMember
    {
        public override MemberDeclarationSyntax MockProperty(string mockPropertyName) => null;
        public override MemberDeclarationSyntax ExplicitInterfaceMember(string mockPropertyName) => null;
        public override TypeSyntax MockPropertyType { get; }
        public override TypeSyntax MockPropertyInterfaceType { get; }

        public MocklisUnused(MocklisClass mocklisClass) : base("__unused__")
        {
            MockPropertyType = mocklisClass.ValueTuple;
            MockPropertyInterfaceType = mocklisClass.ValueTuple;
        }

        public override ExpressionSyntax ConstructorArgument(string uniqueName)
        {
            return F.LiteralExpression(SyntaxKind.DefaultLiteralExpression, F.Token(SyntaxKind.DefaultKeyword));
        }
    }
}
