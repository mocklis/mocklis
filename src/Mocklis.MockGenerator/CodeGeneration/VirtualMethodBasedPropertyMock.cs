// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedPropertyMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class VirtualMethodBasedPropertyMock : IMemberMock
    {
        public IPropertySymbol Symbol { get; }
        public string MemberMockName { get; }

        public VirtualMethodBasedPropertyMock(IPropertySymbol symbol, string mockMemberName)
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
            var (valueType, valueTypeWithoutReadonly) = ctx.FindPropertyTypes(Symbol);

            if (!Symbol.IsWriteOnly)
            {
                ctx.AppendLine($"protected virtual {valueTypeWithoutReadonly} {MemberMockName}()");
                ctx.AppendLine("{");
                ctx.IncreaseIndent();
                ctx.AppendThrow("VirtualPropertyGet", MemberMockName, interfaceSymbol.Name, Symbol.Name);
                ctx.DecreaseIndent();
                ctx.AppendLine("}");
                ctx.AppendSeparator();
            }

            if (!Symbol.IsReadOnly)
            {
                ctx.AppendLine($"protected virtual void {MemberMockName}({valueTypeWithoutReadonly} value)");
                ctx.AppendLine("{");
                ctx.IncreaseIndent();
                ctx.AppendThrow("VirtualPropertySet", MemberMockName, interfaceSymbol.Name, Symbol.Name);
                ctx.DecreaseIndent();
                ctx.AppendLine("}");
                ctx.AppendSeparator();
            }

            ctx.Append($"{valueType} {ctx.ParseTypeName(interfaceSymbol, false, Substitutions.Empty)}.{Symbol.Name}");

            if (Symbol.IsReadOnly)
            {
                ctx.Append(" => ");
                if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
                {
                    ctx.Append("ref ");
                }

                ctx.AppendLine($"{MemberMockName}();");
            }
            else
            {
                ctx.Append(" { ");

                if (!Symbol.IsWriteOnly)
                {
                    ctx.Append($"get => {MemberMockName}(); ");
                }

                if (!Symbol.IsReadOnly)
                {
                    ctx.Append($"set => {MemberMockName}(value); ");
                }

                ctx.AppendLine("}");
            }
        }

        public class SyntaxAdder : ISyntaxAdder
        {
            private readonly VirtualMethodBasedPropertyMock _mock;
            private readonly MocklisTypesForSymbols _typesForSymbols;

            public SyntaxAdder(VirtualMethodBasedPropertyMock mock, MocklisTypesForSymbols typesForSymbols)
            {
                _mock = mock;
                _typesForSymbols = typesForSymbols;
            }

            public void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettingns,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
                string interfaceName)
            {
                var valueTypeWithoutReadonly = _typesForSymbols.ParseTypeName(_mock.Symbol.Type, _mock.Symbol.NullableOrOblivious());
                var valueType = valueTypeWithoutReadonly;

                if (_mock.Symbol.ReturnsByRef || _mock.Symbol.ReturnsByRefReadonly)
                {
                    RefTypeSyntax tmp = F.RefType(valueTypeWithoutReadonly);
                    valueTypeWithoutReadonly = tmp;
                    valueType = tmp;
                    if (_mock.Symbol.ReturnsByRefReadonly)
                    {
                        valueType = tmp.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                    }
                }

                if (!_mock.Symbol.IsWriteOnly)
                {
                    declarationList.Add(MockGetVirtualMethod(valueTypeWithoutReadonly, className, interfaceName));
                }

                if (!_mock.Symbol.IsReadOnly)
                {
                    declarationList.Add(MockSetVirtualMethod(valueTypeWithoutReadonly, className, interfaceName));
                }

                declarationList.Add(ExplicitInterfaceMember(valueType, interfaceNameSyntax));
            }

            public void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
            }

            // TODO: Consider whether a 'default' implementation in lenient mode is to return default values.
            private MemberDeclarationSyntax MockGetVirtualMethod(TypeSyntax valueTypeSyntax, string className, string interfaceName)
            {
                return F.MethodDeclaration(valueTypeSyntax, F.Identifier(_mock.MemberMockName))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithBody(F.Block(_typesForSymbols.ThrowMockMissingStatement("VirtualPropertyGet", _mock.MemberMockName, className, interfaceName,
                        _mock.Symbol.Name)));
            }

            // TODO: Consider whether a 'default' implementation in lenient mode is to do nothing.
            private MemberDeclarationSyntax MockSetVirtualMethod(TypeSyntax valueTypeSyntax, string className, string interfaceName)
            {
                return F.MethodDeclaration(F.PredefinedType(F.Token(SyntaxKind.VoidKeyword)), F.Identifier(_mock.MemberMockName))
                    .WithParameterList(F.ParameterList(F.SeparatedList(new[]
                    {
                        F.Parameter(F.Identifier("value")).WithType(valueTypeSyntax)
                    })))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithBody(F.Block(_typesForSymbols.ThrowMockMissingStatement("VirtualPropertySet", _mock.MemberMockName, className, interfaceName,
                        _mock.Symbol.Name)));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(TypeSyntax valueWithReadonlyTypeSyntax, NameSyntax interfaceNameSyntax)
            {
                var mockedProperty = F.PropertyDeclaration(valueWithReadonlyTypeSyntax, _mock.Symbol.Name)
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

                if (_mock.Symbol.IsReadOnly)
                {
                    ExpressionSyntax invocation = F.InvocationExpression(F.IdentifierName(_mock.MemberMockName));
                    if (_mock.Symbol.ReturnsByRef || _mock.Symbol.ReturnsByRefReadonly)
                    {
                        invocation = F.RefExpression(invocation);
                    }

                    mockedProperty = mockedProperty
                        .WithExpressionBody(F.ArrowExpressionClause(invocation))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
                }
                else
                {
                    if (!_mock.Symbol.IsWriteOnly)
                    {
                        mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(_mock.MemberMockName))))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                        );
                    }

                    if (!_mock.Symbol.IsReadOnly)
                    {
                        mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(_mock.MemberMockName),
                                F.ArgumentList(F.SeparatedList(new[] { F.Argument(F.IdentifierName("value")) })))))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                        );
                    }
                }

                return mockedProperty;
            }
        }
    }
}
