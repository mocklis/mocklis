// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedEventMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class PropertyBasedEventMock : IMemberMock, IEquatable<PropertyBasedEventMock>
    {
        public bool Equals(PropertyBasedEventMock? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return SymbolEqualityComparer.IncludeNullability.Equals(Symbol, other.Symbol) && MemberMockName == other.MemberMockName;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is PropertyBasedEventMock other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (SymbolEqualityComparer.IncludeNullability.GetHashCode(Symbol) * 397) ^ MemberMockName.GetHashCode();
            }
        }

        public static bool operator ==(PropertyBasedEventMock? left, PropertyBasedEventMock? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PropertyBasedEventMock? left, PropertyBasedEventMock? right)
        {
            return !Equals(left, right);
        }

        public IEventSymbol Symbol { get; }
        public string MemberMockName { get; }

        public PropertyBasedEventMock(IEventSymbol symbol, string memberMockName)
        {
            Symbol = symbol;
            MemberMockName = memberMockName;
        }

        public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList, List<StatementSyntax> constructorStatements,
            NameSyntax interfaceNameSyntax, string className, string interfaceName)
        {
            var syntaxAdder = new SyntaxAdder(this, typesForSymbols);
            syntaxAdder.AddMembersToClass(typesForSymbols, declarationList, interfaceNameSyntax);
            syntaxAdder.AddInitialisersToConstructor(typesForSymbols, constructorStatements, className, interfaceName);
        }

        public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
        {
            var interfaceName = ctx.ParseTypeName(interfaceSymbol, false, Substitutions.Empty);
            var eventHandlerType = ctx.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious(), Substitutions.Empty);

            // TODO: Verify that the generic parameter indeed never is nullable. (Pretty sure this is the case but it doesn't hurt to double-check...
            var mockPropertyType = $"global::Mocklis.Core.EventMock<{ctx.ParseTypeName(Symbol.Type, false, Substitutions.Empty)}>";

            ctx.AppendLine($"public {mockPropertyType} {MemberMockName} {{ get; }}");
            ctx.AppendSeparator();

            ctx.AppendLine(
                $"event {eventHandlerType} {interfaceName}.{Symbol.Name} {{ add => {MemberMockName}.Add(value); remove => {MemberMockName}.Remove(value); }}");
            ctx.AppendSeparator();

            ctx.AddConstructorStatement(mockPropertyType, MemberMockName, interfaceSymbol.Name, Symbol.Name);
        }

        private class SyntaxAdder
        {
            private readonly PropertyBasedEventMock _mock;

            private TypeSyntax MockPropertyType { get; }

            public SyntaxAdder(PropertyBasedEventMock mock, MocklisTypesForSymbols typesForSymbols)
            {
                _mock = mock;
                MockPropertyType = typesForSymbols.EventMock(typesForSymbols.ParseTypeName(_mock.Symbol.Type, false));
            }

            public void AddMembersToClass(MocklisTypesForSymbols typesForSymbols,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax)
            {
                declarationList.Add(MockPropertyType.MockProperty(_mock.MemberMockName));
                declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax));
            }

            public void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
                constructorStatements.Add(typesForSymbols.InitialisationStatement(MockPropertyType, _mock.MemberMockName, className, interfaceName,
                    _mock.Symbol.Name));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax)
            {
                var eventHandlerTypeSyntax = typesForSymbols.ParseTypeName(_mock.Symbol.Type, _mock.Symbol.NullableOrOblivious());
                var mockedProperty = F.EventDeclaration(eventHandlerTypeSyntax, _mock.Symbol.Name)
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

                mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.AddAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                            F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(_mock.MemberMockName),
                                F.IdentifierName("Add")))
                        .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                );

                mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.RemoveAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                            F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                F.IdentifierName(_mock.MemberMockName), F.IdentifierName("Remove")))
                        .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));

                return mockedProperty;
            }
        }
    }
}
