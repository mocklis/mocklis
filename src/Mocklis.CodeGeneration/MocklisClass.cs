// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClass.cs">
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

    public sealed class MocklisClass
    {
        private readonly SemanticModel _semanticModel;
        private readonly ClassDeclarationSyntax _classDeclaration;
        private readonly MocklisSymbols _mocklisSymbols;
        private readonly List<MocklisMember> _interfaceMembers = new List<MocklisMember>();

        public MocklisClass(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols)
        {
            _semanticModel = semanticModel;
            _classDeclaration = classDeclaration;
            _mocklisSymbols = mocklisSymbols;
            INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            foreach (var interfaceSymbol in classSymbol.Interfaces)
            {
                foreach (var memberSymbol in interfaceSymbol.GetMembers())
                {
                    if (memberSymbol is IPropertySymbol memberPropertySymbol)
                    {
                        if (memberPropertySymbol.IsIndexer)
                        {
                            _interfaceMembers.Add(new MocklisIndexer(this, interfaceSymbol, memberPropertySymbol));
                        }
                        else
                        {
                            _interfaceMembers.Add(new MocklisProperty(this, interfaceSymbol, memberPropertySymbol));
                        }
                    }
                    else if (memberSymbol is IEventSymbol memberEventSymbol)
                    {
                        _interfaceMembers.Add(new MocklisEvent(this, interfaceSymbol, memberEventSymbol));
                    }
                    else if (memberSymbol is IMethodSymbol methodSymbol && methodSymbol.CanBeReferencedByName)
                    {
                    }
                }
            }
        }

        public void EnsureMockPropertyNamesAreUnique()
        {
        }

        public ClassDeclarationSyntax RewriteClassDeclaration(ClassDeclarationSyntax source)
        {
            var members = new List<MemberDeclarationSyntax> { GenerateConstructor() };
            foreach (var interfaceMember in _interfaceMembers)
            {
                members.Add(interfaceMember.MockProperty());
                members.Add(interfaceMember.ExplicitInterfaceMember());
            }

            return source.WithMembers(F.List(members));
        }

        private MemberDeclarationSyntax GenerateConstructor()
        {
            var parameterType =
                F.TupleType(F.SeparatedList(_interfaceMembers.Select(i =>
                    F.TupleElement(i.MockPropertyType).WithIdentifier(F.Identifier(i.MockPropertyName)))));

            var parameter = F.Parameter(F.Identifier("mockSetup")).WithType(Action(parameterType))
                .WithDefault(F.EqualsValueClause(F.LiteralExpression(SyntaxKind.NullLiteralExpression)));

            var argument = F.Argument(
                F.TupleExpression(F.SeparatedList(_interfaceMembers.Select(i => F.Argument(F.IdentifierName(i.MockPropertyName))))));

            var body = F.Block(F.SingletonList<StatementSyntax>(F.ExpressionStatement(F.ConditionalAccessExpression(F.IdentifierName("mockSetup"),
                F.InvocationExpression(F.MemberBindingExpression(F.IdentifierName("Invoke")))
                    .WithArgumentList(F.ArgumentList(F.SingletonSeparatedList(argument)))))));

            return F.ConstructorDeclaration(_classDeclaration.Identifier)
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(F.ParameterList(F.SingletonSeparatedList(parameter)))
                .WithBody(body);
        }

        public TypeSyntax ParseTypeName(ITypeSymbol propertyType)
        {
            return F.ParseTypeName(propertyType.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart));
        }

        public NameSyntax ParseName(ITypeSymbol propertyType)
        {
            return F.ParseName(propertyType.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart));
        }

        public GenericNameSyntax ParseGenericName(ITypeSymbol symbol, params TypeSyntax[] typeParameters)
        {
            var result = F.ParseName(symbol.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart)) as GenericNameSyntax;
            return result?.WithTypeArgumentList(F.TypeArgumentList(F.SeparatedList(typeParameters)));
        }

        public TypeSyntax ActionMethodMock()
        {
            return ParseTypeName(_mocklisSymbols.ActionMethodMock0);
        }

        public TypeSyntax ActionMethodMock(TypeSyntax tparam)
        {
            return ParseGenericName(_mocklisSymbols.ActionMethodMock1, tparam);
        }

        public TypeSyntax EventMock(TypeSyntax thandler)
        {
            return ParseGenericName(_mocklisSymbols.EventMock1, thandler);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.FuncMethodMock1, tresult);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tparam, TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.FuncMethodMock2, tparam, tresult);
        }

        public TypeSyntax IndexerMock(TypeSyntax tkey, TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.IndexerMock2, tkey, tvalue);
        }

        public TypeSyntax PropertyMock(TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.PropertyMock1, tvalue);
        }

        public TypeSyntax Action(TypeSyntax t)
        {
            return ParseGenericName(_mocklisSymbols.Action1, t);
        }
    }
}
