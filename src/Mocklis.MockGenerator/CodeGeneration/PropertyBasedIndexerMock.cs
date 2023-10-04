// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedIndexerMock.cs">
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

    public sealed class PropertyBasedIndexerMock : IMemberMock, IEquatable<PropertyBasedIndexerMock>
    {
        public bool Equals(PropertyBasedIndexerMock? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return SymbolEqualityComparer.IncludeNullability.Equals(Symbol, other.Symbol) && MemberMockName == other.MemberMockName;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is PropertyBasedIndexerMock other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (SymbolEqualityComparer.IncludeNullability.GetHashCode(Symbol) * 397) ^ MemberMockName.GetHashCode();
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

        public IPropertySymbol Symbol { get; }
        public string MemberMockName { get; }

        public PropertyBasedIndexerMock(IPropertySymbol symbol,
            string mockMemberName)
        {
            Symbol = symbol;
            MemberMockName = mockMemberName;
        }

        public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList, List<StatementSyntax> constructorStatements,
            NameSyntax interfaceNameSyntax, string className, string interfaceName)
        {
            var syntaxAdder = new SyntaxAdder(this, typesForSymbols);
            syntaxAdder.AddMembersToClass(declarationList, interfaceNameSyntax);
            syntaxAdder.AddInitialisersToConstructor(constructorStatements, className, interfaceName);
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

        private class SyntaxAdder
        {
            private readonly PropertyBasedIndexerMock _mock;
            private readonly MocklisTypesForSymbols _typesForSymbols;

            private SingleTypeOrValueTuple KeyType { get; }
            private TypeSyntax KeyTypeSyntax { get; }
            private TypeSyntax ValueTypeSyntax { get; }
            private TypeSyntax MockPropertyType { get; }

            public SyntaxAdder(PropertyBasedIndexerMock mock, MocklisTypesForSymbols typesForSymbols)
            {
                _mock = mock;
                _typesForSymbols = typesForSymbols;

                var builder = new SingleTypeOrValueTupleBuilder();
                foreach (var p in _mock.Symbol.Parameters)
                {
                    builder.AddParameter(p);
                }

                KeyType = builder.Build(_mock.MemberMockName);

                var keyTypeSyntax = KeyType.BuildTypeSyntax(typesForSymbols, null);

                KeyTypeSyntax = keyTypeSyntax ?? throw new ArgumentException("Property symbol must have at least one parameter", nameof(mock));

                ValueTypeSyntax = typesForSymbols.ParseTypeName(_mock.Symbol.Type, _mock.Symbol.NullableOrOblivious());

                MockPropertyType = typesForSymbols.IndexerMock(KeyTypeSyntax, ValueTypeSyntax);
            }

            public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax)
            {
                declarationList.Add(MockPropertyType.MockProperty(_mock.MemberMockName));
                declarationList.Add(ExplicitInterfaceMember(_typesForSymbols, interfaceNameSyntax));
            }

            public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
                constructorStatements.Add(_typesForSymbols.InitialisationStatement(MockPropertyType, _mock.MemberMockName, className, interfaceName,
                    _mock.Symbol.Name));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax)
            {
                var decoratedValueTypeSyntax = ValueTypeSyntax;

                if (_mock.Symbol.ReturnsByRef)
                {
                    decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax);
                }
                else if (_mock.Symbol.ReturnsByRefReadonly)
                {
                    decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                }

                var mockedIndexer = F.IndexerDeclaration(decoratedValueTypeSyntax)
                    .WithParameterList(KeyType.BuildParameterList(typesForSymbols, null))
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

                var arguments = KeyType.BuildArgumentList();

                if (_mock.Symbol.IsReadOnly)
                {
                    ExpressionSyntax elementAccess = F.ElementAccessExpression(F.IdentifierName(_mock.MemberMockName))
                        .WithExpressionsAsArgumentList(arguments);

                    if (_mock.Symbol.ReturnsByRef || _mock.Symbol.ReturnsByRefReadonly)
                    {
                        elementAccess = typesForSymbols.WrapByRef(elementAccess, ValueTypeSyntax);
                    }

                    mockedIndexer = mockedIndexer.WithExpressionBody(F.ArrowExpressionClause(elementAccess))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
                }
                else
                {
                    if (!_mock.Symbol.IsWriteOnly)
                    {
                        mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(F.ElementAccessExpression(F.IdentifierName(_mock.MemberMockName))
                                .WithExpressionsAsArgumentList(arguments)))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                        );
                    }

                    if (!_mock.Symbol.IsReadOnly)
                    {
                        mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(
                                F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    F.ElementAccessExpression(F.IdentifierName(_mock.MemberMockName)).WithExpressionsAsArgumentList(arguments),
                                    F.IdentifierName("value"))))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
                    }
                }

                return mockedIndexer;
            }
        }
    }
}
