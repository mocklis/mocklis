// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedIndexerMock.cs">
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

public sealed class PropertyBasedIndexerMock : IMemberMock, IEquatable<PropertyBasedIndexerMock>
{
    #region Equality members

    public bool Equals(PropertyBasedIndexerMock? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return SymbolEquality.ForProperty.Equals(Symbol, other.Symbol) && MemberMockName == other.MemberMockName;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyBasedIndexerMock other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (SymbolEquality.ForProperty.GetHashCode(Symbol) * 397) ^ MemberMockName.GetHashCode();
        }
    }

    public static bool operator ==(PropertyBasedIndexerMock? left, PropertyBasedIndexerMock? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PropertyBasedIndexerMock? left, PropertyBasedIndexerMock? right)
    {
        return !Equals(left, right);
    }

    #endregion

    public IPropertySymbol Symbol { get; }
    public string MemberMockName { get; }

    public PropertyBasedIndexerMock(IPropertySymbol symbol,
        string mockMemberName)
    {
        Symbol = symbol;
        MemberMockName = mockMemberName;
    }

    public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
    {
        var interfaceName = ctx.ParseTypeName(interfaceSymbol, false, Substitutions.Empty);

        var builder = new SingleTypeOrValueTupleBuilder();
        foreach (var p in Symbol.Parameters)
        {
            builder.AddParameter(p);
        }

        var keyTypex = builder.Build(MemberMockName);

        var keyType = ctx.BuildTupleType(keyTypex, Substitutions.Empty) ??
                      throw new ArgumentException("Indexer symbol must have at least one parameter", nameof(Symbol));

        var arglist = keyTypex.BuildArgumentListAsString();

        var valueType = ctx.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious(), Substitutions.Empty);

        var mockPropertyType = $"global::Mocklis.Core.IndexerMock<{keyType}, {valueType}>";

        ctx.AppendLine($"public {mockPropertyType} {MemberMockName} {{ get; }}");

        ctx.AppendSeparator();

        if (Symbol.ReturnsByRef)
        {
            ctx.Append("ref ");
        }
        else if (Symbol.ReturnsByRefReadonly)
        {
            ctx.Append("ref readonly ");
        }

        ctx.Append($"{valueType} {interfaceName}.this[{ctx.BuildParameterList(Symbol.Parameters, Substitutions.Empty)}]");

        if (Symbol.IsReadOnly)
        {
            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                ctx.AppendLine($" => ref global::Mocklis.Core.ByRef<{valueType}>.Wrap({MemberMockName}[{arglist}]);");
            }
            else
            {
                ctx.AppendLine($" => {MemberMockName}[{arglist}];");
            }
        }
        else
        {
            ctx.Append(" { ");

            if (!Symbol.IsWriteOnly)
            {
                ctx.Append($"get => {MemberMockName}[{arglist}]; ");
            }

            if (!Symbol.IsReadOnly)
            {
                ctx.Append($"set => {MemberMockName}[{arglist}] = value; ");
            }

            ctx.Append("}");
        }

        ctx.AppendLine();
        ctx.AppendSeparator();

        ctx.AddConstructorStatement(mockPropertyType, MemberMockName, interfaceSymbol.Name, Symbol.Name);
    }

    public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements,
        NameSyntax interfaceNameSyntax, string className, string interfaceName)
    {
        var builder = new SingleTypeOrValueTupleBuilder();
        foreach (var p in Symbol.Parameters)
        {
            builder.AddParameter(p);
        }

        var keyType = builder.Build(MemberMockName);

        var keyTypeSyntax = keyType.BuildTypeSyntax(typesForSymbols, null) ?? throw new ArgumentException("Property symbol must have at least one parameter", nameof(Symbol));

        var valueTypeSyntax = typesForSymbols.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious());

        var mockPropertyType = typesForSymbols.IndexerMock(keyTypeSyntax, valueTypeSyntax);

        declarationList.Add(mockPropertyType.MockProperty(MemberMockName));
        declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax, valueTypeSyntax, keyType));

        constructorStatements.Add(
            typesForSymbols.InitialisationStatement(mockPropertyType, MemberMockName, className, interfaceName, Symbol.Name));

    }

    private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax,
        TypeSyntax valueTypeSyntax, SingleTypeOrValueTuple keyType)
    {
        var decoratedValueTypeSyntax = valueTypeSyntax;

        if (Symbol.ReturnsByRef)
        {
            decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax);
        }
        else if (Symbol.ReturnsByRefReadonly)
        {
            decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
        }

        var mockedIndexer = F.IndexerDeclaration(decoratedValueTypeSyntax)
            .WithParameterList(keyType.BuildParameterList(typesForSymbols, null))
            .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

        var arguments = keyType.BuildArgumentList();

        if (Symbol.IsReadOnly)
        {
            ExpressionSyntax elementAccess = F.ElementAccessExpression(F.IdentifierName(MemberMockName))
                .WithExpressionsAsArgumentList(arguments);

            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                elementAccess = typesForSymbols.WrapByRef(elementAccess, valueTypeSyntax);
            }

            mockedIndexer = mockedIndexer.WithExpressionBody(F.ArrowExpressionClause(elementAccess))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
        }
        else
        {
            if (!Symbol.IsWriteOnly)
            {
                mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(F.ElementAccessExpression(F.IdentifierName(MemberMockName))
                        .WithExpressionsAsArgumentList(arguments)))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                );
            }

            if (!Symbol.IsReadOnly)
            {
                mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(
                        F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                            F.ElementAccessExpression(F.IdentifierName(MemberMockName)).WithExpressionsAsArgumentList(arguments),
                            F.IdentifierName("value"))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
            }
        }

        return mockedIndexer;
    }
}
