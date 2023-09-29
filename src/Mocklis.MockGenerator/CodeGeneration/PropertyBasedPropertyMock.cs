// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedPropertyMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.MockGenerator.CodeGeneration;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class PropertyBasedPropertyMock : IMemberMock
    {
        protected IPropertySymbol Symbol { get; }
        protected string MemberMockName { get; }

        public PropertyBasedPropertyMock(IPropertySymbol symbol, string mockMemberName)
        {
            Symbol = symbol;
            MemberMockName = mockMemberName;
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
        {
            return new SyntaxAdder(this, typesForSymbols);
        }

        public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
        {
            var interfaceName = ctx.ParseTypeName(interfaceSymbol, false, ITypeParameterSubstitutions.Empty);
            var valueType = ctx.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious(), ITypeParameterSubstitutions.Empty);

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

        private class SyntaxAdder : ISyntaxAdder
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

            public void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettingns,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
                string interfaceName)
            {
                declarationList.Add(MockPropertyType.MockProperty(Mock.MemberMockName));
                declarationList.Add(ExplicitInterfaceMember(interfaceNameSyntax));
            }

            public void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
                constructorStatements.Add(TypesForSymbols.InitialisationStatement(MockPropertyType, Mock.MemberMockName, className, interfaceName, Mock.Symbol.Name, mockSettings.Strict, mockSettings.VeryStrict));
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
                    ExpressionSyntax elementAccess = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(Mock.MemberMockName),
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
}
