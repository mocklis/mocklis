// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisTypesForSymbols.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.MockGenerator.CodeGeneration.Compatibility;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class MocklisTypesForSymbols
    {
        private readonly SemanticModel _semanticModel;
        private readonly ClassDeclarationSyntax _classDeclaration;
        private readonly MocklisSymbols _mocklisSymbols;
        private readonly bool _nullableContextEnabled;

        private static readonly SymbolDisplayFormat SymbolDisplayFormat = SymbolDisplayFormat.MinimallyQualifiedFormat.WithMiscellaneousOptions(
            SymbolDisplayFormat.MinimallyQualifiedFormat.MiscellaneousOptions | SymbolDisplayMiscellaneousOptions.RemoveAttributeSuffix);

        private static readonly string Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

        public MocklisTypesForSymbols(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols,
            bool nullableContextEnabled)
        {
            _semanticModel = semanticModel;
            _classDeclaration = classDeclaration;
            _mocklisSymbols = mocklisSymbols;
            _nullableContextEnabled = nullableContextEnabled;
        }

        public TypeSyntax ParseTypeName(ITypeSymbol typeSymbol, bool makeNullableIfPossible, Func<string, string>? findTypeParameterName = null)
        {
            var x = typeSymbol.ToMinimalDisplayParts(_semanticModel, _classDeclaration.SpanStart, SymbolDisplayFormat);

            string s = string.Empty;
            foreach (var part in x)
            {
                switch (part.Kind)
                {
                    case SymbolDisplayPartKind.TypeParameterName:
                    {
                        var partSymbol = part.Symbol;

                        if (partSymbol is { ContainingSymbol: IMethodSymbol methodSymbol } && methodSymbol.TypeParameters.Contains(partSymbol, SymbolEqualityComparer.Default))
                        {
                            if (findTypeParameterName is null)
                            {
                                s += partSymbol.Name;
                            }
                            else
                            {
                                s += findTypeParameterName(partSymbol.Name);
                            }
                        }
                        else
                        {
                            s += part.ToString();
                        }

                        break;
                    }

                    default:
                    {
                        s += part.ToString();
                        break;
                    }
                }
            }

            // Fix for GitHub issue #17. There are cases when the ToMinimalDisplayParts incorrectly adds a SymbolDisplayPartKind.Punctuation
            // containing a "?" at the end of the type name for a reference type. If so we just remove it from the type name.
            // Note that these punctuations are perfectly valid as part of the type name, such as "List<int?>".
            if (s.EndsWith("?") && typeSymbol.IsReferenceType)
            {
                s = s.Substring(0, s.Length - 1);
            }

            var parsedTypeName = F.ParseTypeName(s);
            if (makeNullableIfPossible && _nullableContextEnabled && typeSymbol.IsReferenceType)
            {
                return F.NullableType(parsedTypeName);
            }

            return parsedTypeName;
        }

        public NameSyntax ParseName(ITypeSymbol symbol)
        {
            return F.ParseName(symbol.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart, SymbolDisplayFormat));
        }

        private TypeSyntax ParseGenericType(ITypeSymbol symbol, Func<string, string>? findTypeParameterName, params TypeSyntax[] typeParameters)
        {
            TypeSyntax ApplyTypeParameters(TypeSyntax typeSyntax)
            {
                if (typeSyntax is GenericNameSyntax genericNameSyntax)
                {
                    return genericNameSyntax.WithTypeArgumentList(F.TypeArgumentList(F.SeparatedList(typeParameters)));
                }

                if (typeSyntax is QualifiedNameSyntax qualifiedNameSyntax)
                {
                    return qualifiedNameSyntax.WithRight((SimpleNameSyntax)ApplyTypeParameters(qualifiedNameSyntax.Right));
                }

                return typeSyntax;
            }

            return ApplyTypeParameters(ParseTypeName(symbol, false, findTypeParameterName));
        }

        public TypeSyntax ActionMethodMock()
        {
            return ParseTypeName(_mocklisSymbols.ActionMethodMock0, false);
        }

        public TypeSyntax ActionMethodMock(TypeSyntax tparam, Func<string, string>? findTypeParameterName = null)
        {
            return ParseGenericType(_mocklisSymbols.ActionMethodMock1, findTypeParameterName, tparam);
        }

        public TypeSyntax EventMock(TypeSyntax thandler)
        {
            return ParseGenericType(_mocklisSymbols.EventMock1, null, thandler);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tresult, Func<string, string>? findTypeParameterName = null)
        {
            return ParseGenericType(_mocklisSymbols.FuncMethodMock1, findTypeParameterName, tresult);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tparam, TypeSyntax tresult, Func<string, string>? findTypeParameterName = null)
        {
            return ParseGenericType(_mocklisSymbols.FuncMethodMock2, findTypeParameterName, tparam, tresult);
        }

        public TypeSyntax IndexerMock(TypeSyntax tkey, TypeSyntax tvalue)
        {
            return ParseGenericType(_mocklisSymbols.IndexerMock2, null, tkey, tvalue);
        }

        public TypeSyntax PropertyMock(TypeSyntax tvalue)
        {
            return ParseGenericType(_mocklisSymbols.PropertyMock1, null, tvalue);
        }

        public TypeSyntax MockMissingException() => ParseTypeName(_mocklisSymbols.MockMissingException, false);


        public TypeSyntax MockType() => ParseTypeName(_mocklisSymbols.MockType, false);

        public TypeSyntax ByRef(TypeSyntax tresult)
        {
            return ParseGenericType(_mocklisSymbols.ByRef1, null, tresult);
        }

        public TypeSyntax TypedMockProvider() => ParseTypeName(_mocklisSymbols.TypedMockProvider, false);

        public TypeSyntax RuntimeArgumentHandle() => ParseTypeName(_mocklisSymbols.RuntimeArgumentHandle, false);

        public MemberAccessExpressionSyntax StrictnessLenient() => F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            ParseTypeName(_mocklisSymbols.Strictness, false), F.IdentifierName("Lenient"));

        public MemberAccessExpressionSyntax StrictnessStrict() => F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            ParseTypeName(_mocklisSymbols.Strictness, false), F.IdentifierName("Strict"));

        public MemberAccessExpressionSyntax StrictnessVeryStrict() => F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            ParseTypeName(_mocklisSymbols.Strictness, false), F.IdentifierName("VeryStrict"));

        public ParameterSyntax AsParameterSyntax(IParameterSymbol p, Func<string, string>? findTypeParameterName = null)
        {
            var syntax = F.Parameter(F.Identifier(p.Name)).WithType(ParseTypeName(p.Type, p.NullableOrOblivious(), findTypeParameterName));

            switch (p.RefKind)
            {
                case RefKind.In:
                {
                    syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.InKeyword)));
                    break;
                }

                case RefKind.Out:
                {
                    syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.OutKeyword)));
                    break;
                }

                case RefKind.Ref:
                {
                    syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.RefKeyword)));
                    break;
                }
            }

            return syntax;
        }

        private TypeParameterConstraintClauseSyntax? CreateClassConstraintClausesForReferenceTypeParameter(ITypeParameterSymbol typeParameter, Func<string, string> findTypeParameterName)
        {
            if (_nullableContextEnabled)
            {
                var constraints = new List<TypeParameterConstraintSyntax>();

                if (typeParameter.IsReferenceType)
                {
                    constraints.Add(F.ClassOrStructConstraint(SyntaxKind.ClassConstraint));
                }

                if (constraints.Any())
                {
                    var name = findTypeParameterName(typeParameter.Name);
                    return F.TypeParameterConstraintClause(F.IdentifierName(name), F.SeparatedList(constraints));
                }
            }

            // TODO: Clearly we want to do something here even if we're not in a nullable context?

            return null;
        }

        private TypeParameterConstraintClauseSyntax? CreateConstraintClauseFromTypeParameter(ITypeParameterSymbol typeParameter, Func<string, string> findTypeParameterName)
        {
            var constraints = new List<TypeParameterConstraintSyntax>();

            if (typeParameter.HasReferenceTypeConstraint)
            {
                constraints.Add(F.ClassOrStructConstraint(SyntaxKind.ClassConstraint));
            }

            // Note that 'unmanaged' is a type of valuetype constraint.
            if (typeParameter.HasUnmanagedTypeConstraint)
            {
                constraints.Add(F.TypeConstraint(F.IdentifierName("unmanaged")));
            }
            else if (typeParameter.HasValueTypeConstraint)
            {
                constraints.Add(F.ClassOrStructConstraint(SyntaxKind.StructConstraint));
            }

            foreach (var type in typeParameter.ConstraintTypes)
            {
                if (!type.IsValueType)
                {
                    constraints.Add(F.TypeConstraint(ParseTypeName(type, false)));
                }
            }

            if (typeParameter.HasConstructorConstraint)
            {
                constraints.Add(F.ConstructorConstraint());
            }

            if (typeParameter.HasNotNullConstraint() && _nullableContextEnabled)
            {
                constraints.Add(F.TypeConstraint(F.IdentifierName("notnull")));
            }

            if (constraints.Any())
            {
                var name = findTypeParameterName(typeParameter.Name);
                return F.TypeParameterConstraintClause(F.IdentifierName(name), F.SeparatedList(constraints));
            }

            return null;
        }

        public TypeParameterConstraintClauseSyntax[] AsClassConstraintClausesForReferenceTypes(IEnumerable<ITypeParameterSymbol> typeParameters, Func<string, string> findTypeParameterName)
        {
            return typeParameters
                .Select(a => CreateClassConstraintClausesForReferenceTypeParameter(a, findTypeParameterName))
                .Where(a => a != null)
                .Select(a => a!)
                .ToArray();
        }

        public TypeParameterConstraintClauseSyntax[] AsConstraintClauses(IEnumerable<ITypeParameterSymbol> typeParameters, Func<string, string> findTypeParameterName)
        {
            return typeParameters
                .Select(a => CreateConstraintClauseFromTypeParameter(a, findTypeParameterName))
                .Where(a => a != null)
                .Select(a => a!)
                .ToArray();
        }

        public ExpressionSyntax WrapByRef(ExpressionSyntax invocation, TypeSyntax returnType)
        {
            var byref = ByRef(returnType);
            var wrap = F.InvocationExpression(F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, byref, F.IdentifierName("Wrap")))
                .WithExpressionsAsArgumentList(invocation);
            return F.RefExpression(wrap);
        }

        public AttributeSyntax GeneratedCodeAttribute()
        {
            var p = ParseName(_mocklisSymbols.GeneratedCodeAttribute);

            return F.Attribute(p).WithArgumentList(F.AttributeArgumentList(
                F.SeparatedList(new[]
                {
                    F.AttributeArgument(F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal("Mocklis"))),
                    F.AttributeArgument(F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Version)))
                })));
        }

        public MemberAccessExpressionSyntax StrictnessExpression(bool strict, bool veryStrict)
        {
            return veryStrict ? StrictnessVeryStrict() : strict ? StrictnessStrict() : StrictnessLenient();
        }

    }
}
