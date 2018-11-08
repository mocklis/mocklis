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
        private readonly (string uniqueName, MocklisMember item)[] _interfaceMembers;

        public MocklisClass(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols)
        {
            _semanticModel = semanticModel;
            _classDeclaration = classDeclaration;
            _mocklisSymbols = mocklisSymbols;
            ValueTuple = ParseName(mocklisSymbols.ValueTuple);
            INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            _interfaceMembers = Uniquifier.GetUniqueNames(FindAllMembersInClass(classSymbol)).ToArray();
        }

        private IEnumerable<MocklisMember> FindAllMembersInClass(INamedTypeSymbol classSymbol)
        {
            foreach (var interfaceSymbol in classSymbol.AllInterfaces)
            {
                foreach (var memberSymbol in interfaceSymbol.GetMembers())
                {
                    if (memberSymbol is IPropertySymbol memberPropertySymbol)
                    {
                        if (memberPropertySymbol.IsIndexer)
                        {
                            yield return new MocklisIndexer(this, interfaceSymbol, memberPropertySymbol);
                        }
                        else
                        {
                            yield return new MocklisProperty(this, interfaceSymbol, memberPropertySymbol);
                        }
                    }
                    else if (memberSymbol is IEventSymbol memberEventSymbol)
                    {
                        yield return new MocklisEvent(this, interfaceSymbol, memberEventSymbol);
                    }
                    else if (memberSymbol is IMethodSymbol memberMethodSymbol && memberMethodSymbol.CanBeReferencedByName)
                    {
                        yield return new MocklisMethod(this, interfaceSymbol, memberMethodSymbol);
                    }
                }
            }
        }

        public IEnumerable<MemberDeclarationSyntax> GenerateMembers()
        {
            yield return GenerateConstructor();
            foreach (var interfaceMember in _interfaceMembers)
            {
                yield return interfaceMember.item.MockProperty(interfaceMember.uniqueName);

                var x = interfaceMember.item.ExplicitInterfaceMember(interfaceMember.uniqueName);
                if (x != null)
                {
                    yield return x;
                }
            }
        }

        private MemberDeclarationSyntax GenerateConstructor()
        {
            var parameterType =
                F.TupleType(F.SeparatedList(_interfaceMembers.Select(i =>
                    F.TupleElement(i.item.MockPropertyInterfaceType).WithIdentifier(F.Identifier(i.uniqueName)))));

            var parameter = F.Parameter(F.Identifier("mockSetup")).WithType(Action(parameterType))
                .WithDefault(F.EqualsValueClause(F.LiteralExpression(SyntaxKind.NullLiteralExpression)));

            var argument = F.Argument(
                F.TupleExpression(F.SeparatedList(_interfaceMembers.Select(i => F.Argument(F.IdentifierName(i.uniqueName))))));

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

        public TypeSyntax ValueTuple { get; }

        public TypeSyntax EventStepCallerMock(TypeSyntax thandler)
        {
            return ParseGenericName(_mocklisSymbols.EventStepCaller1, thandler);
        }

        public TypeSyntax IndexerStepCallerMock(TypeSyntax tkey, TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.IndexerStepCaller2, tkey, tvalue);
        }

        public TypeSyntax MethodStepCallerMock(TypeSyntax tparam, TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.MethodStepCaller2, tparam, tresult);
        }

        public TypeSyntax PropertyStepCallerMock(TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.PropertyStepCaller1, tvalue);
        }
    }
}
