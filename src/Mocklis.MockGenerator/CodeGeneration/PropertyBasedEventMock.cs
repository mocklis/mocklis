// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedEventMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

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
    #region Equality members

    public bool Equals(PropertyBasedEventMock? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return SymbolEquality.ForEvent.Equals(Symbol, other.Symbol) && MemberMockName == other.MemberMockName;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyBasedEventMock other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (SymbolEquality.ForEvent.GetHashCode(Symbol) * 397) ^ MemberMockName.GetHashCode();
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

    #endregion

    public IEventSymbol Symbol { get; }
    public string MemberMockName { get; }

    public PropertyBasedEventMock(IEventSymbol symbol, string memberMockName)
    {
        Symbol = symbol;
        MemberMockName = memberMockName;
    }

    public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
    {
        var interfaceName = ctx.ParseTypeName(interfaceSymbol, false, Substitutions.Empty);
        var eventHandlerType = ctx.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious(), Substitutions.Empty);

        // TODO: Verify that the generic parameter indeed never is nullable. (Pretty sure this is the case but it doesn't hurt to double-check...)
        var mockPropertyType = $"global::Mocklis.Core.EventMock<{ctx.ParseTypeName(Symbol.Type, false, Substitutions.Empty)}>";

        ctx.AppendLine($"public {mockPropertyType} {MemberMockName} {{ get; }}");
        ctx.AppendSeparator();

        ctx.AppendLine(
            $"event {eventHandlerType} {interfaceName}.{Symbol.Name} {{ add => {MemberMockName}.Add(value); remove => {MemberMockName}.Remove(value); }}");
        ctx.AppendSeparator();

        ctx.AddConstructorStatement(mockPropertyType, MemberMockName, interfaceSymbol.Name, Symbol.Name);
    }

    public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList, List<StatementSyntax> constructorStatements,
        NameSyntax interfaceNameSyntax, string className, string interfaceName)
    {
        var mockPropertyType = typesForSymbols.EventMock(typesForSymbols.ParseTypeName(Symbol.Type, false));
        declarationList.Add(mockPropertyType.MockProperty(MemberMockName));
        declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax));
        constructorStatements.Add(typesForSymbols.InitialisationStatement(mockPropertyType, MemberMockName, className, interfaceName, Symbol.Name));
    }

    private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax)
    {
        var eventHandlerTypeSyntax = typesForSymbols.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious());
        var mockedProperty = F.EventDeclaration(eventHandlerTypeSyntax, Symbol.Name)
            .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

        mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.AddAccessorDeclaration)
            .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(MemberMockName),
                        F.IdentifierName("Add")))
                .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
        );

        mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.RemoveAccessorDeclaration)
            .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        F.IdentifierName(MemberMockName), F.IdentifierName("Remove")))
                .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));

        return mockedProperty;
    }
}
