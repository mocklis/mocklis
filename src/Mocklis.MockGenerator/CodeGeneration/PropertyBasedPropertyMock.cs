// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedPropertyMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
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

public sealed class PropertyBasedPropertyMock : IMemberMock, IEquatable<PropertyBasedPropertyMock>
{
    #region Equality Members

    public bool Equals(PropertyBasedPropertyMock? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return SymbolEquality.ForProperty.Equals(Symbol, other.Symbol) && MemberMockName == other.MemberMockName;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is PropertyBasedPropertyMock other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (SymbolEquality.ForProperty.GetHashCode(Symbol) * 397) ^ MemberMockName.GetHashCode();
        }
    }

    public static bool operator ==(PropertyBasedPropertyMock? left, PropertyBasedPropertyMock? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PropertyBasedPropertyMock? left, PropertyBasedPropertyMock? right)
    {
        return !Equals(left, right);
    }

    #endregion

    public IPropertySymbol Symbol { get; }
    public string MemberMockName { get; }

    public PropertyBasedPropertyMock(IPropertySymbol symbol, string mockMemberName)
    {
        Symbol = symbol;
        MemberMockName = mockMemberName;
    }

    public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
    {
        var interfaceName = ctx.ParseTypeName(interfaceSymbol, false, Substitutions.Empty);
        var valueType = ctx.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious(), Substitutions.Empty);

        var mockPropertyType = $"global::Mocklis.Core.PropertyMock<{valueType}>";

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

        ctx.Append($"{valueType} {interfaceName}.{Symbol.Name}");

        if (Symbol.IsReadOnly)
        {
            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                ctx.AppendLine($" => ref global::Mocklis.Core.ByRef<{valueType}>.Wrap({MemberMockName}.Value);");
            }
            else
            {
                ctx.AppendLine($" => {MemberMockName}.Value;");
            }
        }
        else
        {
            ctx.Append(" { ");

            if (!Symbol.IsWriteOnly)
            {
                ctx.Append($"get => {MemberMockName}.Value; ");
            }

            if (!Symbol.IsReadOnly)
            {
                ctx.Append($"set => {MemberMockName}.Value = value; ");
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
        var syntaxAdder = new SyntaxAdder(this, typesForSymbols);
        syntaxAdder.AddMembersToClass(declarationList, interfaceNameSyntax);
        syntaxAdder.AddInitialisersToConstructor(constructorStatements, className, interfaceName);
    }

    private class SyntaxAdder
    {
        private TypeSyntax ValueTypeSyntax { get; }
        private TypeSyntax MockPropertyType { get; }
        private MocklisTypesForSymbols TypesForSymbols { get; }
        private PropertyBasedPropertyMock Mock { get; }

        public SyntaxAdder(PropertyBasedPropertyMock mock, MocklisTypesForSymbols typesForSymbols)
        {
            TypesForSymbols = typesForSymbols;
            Mock = mock;
            ValueTypeSyntax = typesForSymbols.ParseTypeName(mock.Symbol.Type, mock.Symbol.NullableOrOblivious());
            MockPropertyType = typesForSymbols.PropertyMock(ValueTypeSyntax);
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax)
        {
            declarationList.Add(MockPropertyType.MockProperty(Mock.MemberMockName));
            declarationList.Add(ExplicitInterfaceMember(interfaceNameSyntax));
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, string className, string interfaceName)
        {
            constructorStatements.Add(TypesForSymbols.InitialisationStatement(MockPropertyType, Mock.MemberMockName, className, interfaceName,
                Mock.Symbol.Name));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember(NameSyntax interfaceNameSyntax)
        {
            var decoratedValueTypeSyntax = ValueTypeSyntax;

            if (Mock.Symbol.ReturnsByRef)
            {
                decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax);
            }
            else if (Mock.Symbol.ReturnsByRefReadonly)
            {
                decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
            }

            var mockedProperty = F.PropertyDeclaration(decoratedValueTypeSyntax, Mock.Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

            if (Mock.Symbol.IsReadOnly)
            {
                ExpressionSyntax elementAccess = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    F.IdentifierName(Mock.MemberMockName),
                    F.IdentifierName("Value"));

                if (Mock.Symbol.ReturnsByRef || Mock.Symbol.ReturnsByRefReadonly)
                {
                    elementAccess = TypesForSymbols.WrapByRef(elementAccess, ValueTypeSyntax);
                }

                mockedProperty = mockedProperty.WithExpressionBody(F.ArrowExpressionClause(elementAccess))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                if (!Mock.Symbol.IsWriteOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            F.IdentifierName(Mock.MemberMockName),
                            F.IdentifierName("Value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }

                if (!Mock.Symbol.IsReadOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithExpressionBody(
                            F.ArrowExpressionClause(
                                F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(Mock.MemberMockName),
                                        F.IdentifierName("Value")),
                                    F.IdentifierName("value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }
            }

            return mockedProperty;
        }
    }
}
