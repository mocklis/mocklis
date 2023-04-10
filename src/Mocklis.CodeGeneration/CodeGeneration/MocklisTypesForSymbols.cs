// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisTypesForSymbols.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class MocklisTypesForSymbols
    {
        private readonly SemanticModel _semanticModel;
        private readonly ClassDeclarationSyntax _classDeclaration;
        private readonly MocklisSymbols _mocklisSymbols;
        private readonly bool _nullableContextEnabled;
        private readonly Dictionary<string, string>? _typeParameterNameSubstitutions;

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
            _typeParameterNameSubstitutions = null;
        }

        public MocklisTypesForSymbols WithSubstitutions(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol)
        {
            return new MocklisTypesForSymbols(_semanticModel, _classDeclaration, _mocklisSymbols, classSymbol, methodSymbol, _nullableContextEnabled);
        }

        private MocklisTypesForSymbols(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols,
            INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol, bool nullableContextEnabled)
        {
            _semanticModel = semanticModel;
            _classDeclaration = classDeclaration;
            _mocklisSymbols = mocklisSymbols;
            _typeParameterNameSubstitutions = new Dictionary<string, string>();
            _nullableContextEnabled = nullableContextEnabled;
            Uniquifier t = new Uniquifier(classSymbol.TypeParameters.Select(tp => tp.Name));

            foreach (var methodTypeParameter in methodSymbol.TypeParameters)
            {
                string uniqueName = t.GetUniqueName(methodTypeParameter.Name);
                _typeParameterNameSubstitutions[methodTypeParameter.Name] = uniqueName;
            }
        }

        public string FindTypeParameterName(string typeParameterName)
        {
            return _typeParameterNameSubstitutions != null && _typeParameterNameSubstitutions.ContainsKey(typeParameterName)
                ? _typeParameterNameSubstitutions[typeParameterName]
                : typeParameterName;
        }

        public TypeSyntax ParseTypeName(ITypeSymbol typeSymbol, bool makeNullableIfPossible)
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
                            s += FindTypeParameterName(partSymbol.Name);
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

        private TypeSyntax ParseGenericType(ITypeSymbol symbol, params TypeSyntax[] typeParameters)
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

            return ApplyTypeParameters(ParseTypeName(symbol, false));
        }

        public TypeSyntax ActionMethodMock()
        {
            return ParseTypeName(_mocklisSymbols.ActionMethodMock0, false);
        }

        public TypeSyntax ActionMethodMock(TypeSyntax tparam)
        {
            return ParseGenericType(_mocklisSymbols.ActionMethodMock1, tparam);
        }

        public TypeSyntax EventMock(TypeSyntax thandler)
        {
            return ParseGenericType(_mocklisSymbols.EventMock1, thandler);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tresult)
        {
            return ParseGenericType(_mocklisSymbols.FuncMethodMock1, tresult);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tparam, TypeSyntax tresult)
        {
            return ParseGenericType(_mocklisSymbols.FuncMethodMock2, tparam, tresult);
        }

        public TypeSyntax IndexerMock(TypeSyntax tkey, TypeSyntax tvalue)
        {
            return ParseGenericType(_mocklisSymbols.IndexerMock2, tkey, tvalue);
        }

        public TypeSyntax PropertyMock(TypeSyntax tvalue)
        {
            return ParseGenericType(_mocklisSymbols.PropertyMock1, tvalue);
        }

        public TypeSyntax MockMissingException() => ParseTypeName(_mocklisSymbols.MockMissingException, false);


        public TypeSyntax MockType() => ParseTypeName(_mocklisSymbols.MockType, false);

        public TypeSyntax ByRef(TypeSyntax tresult)
        {
            return ParseGenericType(_mocklisSymbols.ByRef1, tresult);
        }

        public TypeSyntax TypedMockProvider() => ParseTypeName(_mocklisSymbols.TypedMockProvider, false);

        public TypeSyntax RuntimeArgumentHandle() => ParseTypeName(_mocklisSymbols.RuntimeArgumentHandle, false);

        public MemberAccessExpressionSyntax StrictnessLenient() => F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            ParseTypeName(_mocklisSymbols.Strictness, false), F.IdentifierName("Lenient"));

        public MemberAccessExpressionSyntax StrictnessStrict() => F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            ParseTypeName(_mocklisSymbols.Strictness, false), F.IdentifierName("Strict"));

        public MemberAccessExpressionSyntax StrictnessVeryStrict() => F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            ParseTypeName(_mocklisSymbols.Strictness, false), F.IdentifierName("VeryStrict"));

        public ParameterSyntax AsParameterSyntax(IParameterSymbol p)
        {
            var syntax = F.Parameter(F.Identifier(p.Name)).WithType(ParseTypeName(p.Type, p.NullableOrOblivious()));

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

        private TypeParameterConstraintClauseSyntax? CreateClassConstraintClausesForReferenceTypeParameter(ITypeParameterSymbol typeParameter)
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
                    var name = FindTypeParameterName(typeParameter.Name);
                    return F.TypeParameterConstraintClause(F.IdentifierName(name), F.SeparatedList(constraints));
                }
            }

            return null;
        }

        private TypeParameterConstraintClauseSyntax? CreateConstraintClauseFromTypeParameter(ITypeParameterSymbol typeParameter)
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
                var name = FindTypeParameterName(typeParameter.Name);
                return F.TypeParameterConstraintClause(F.IdentifierName(name), F.SeparatedList(constraints));
            }

            return null;
        }

        public TypeParameterConstraintClauseSyntax[] AsClassConstraintClausesForReferenceTypes(IEnumerable<ITypeParameterSymbol> typeParameters)
        {
            return typeParameters
                .Select(CreateClassConstraintClausesForReferenceTypeParameter)
                .Where(a => a != null)
                .Select(a => a!)
                .ToArray();
        }

        public TypeParameterConstraintClauseSyntax[] AsConstraintClauses(IEnumerable<ITypeParameterSymbol> typeParameters)
        {
            return typeParameters
                .Select(CreateConstraintClauseFromTypeParameter)
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
