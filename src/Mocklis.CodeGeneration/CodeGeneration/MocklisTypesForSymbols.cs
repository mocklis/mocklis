// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisTypesForSymbols.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class MocklisTypesForSymbols
    {
        private readonly SemanticModel _semanticModel;
        private readonly ClassDeclarationSyntax _classDeclaration;
        private readonly MocklisSymbols _mocklisSymbols;
        private readonly Dictionary<string, string> _typeParameterNameSubstitutions;

        public MocklisTypesForSymbols(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols)
        {
            _semanticModel = semanticModel;
            _classDeclaration = classDeclaration;
            _mocklisSymbols = mocklisSymbols;
            _typeParameterNameSubstitutions = null;
        }

        public MocklisTypesForSymbols WithSubstitutions(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol)
        {
            return new MocklisTypesForSymbols(_semanticModel, _classDeclaration, _mocklisSymbols, classSymbol, methodSymbol);
        }

        private MocklisTypesForSymbols(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols,
            INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol)
        {
            _semanticModel = semanticModel;
            _classDeclaration = classDeclaration;
            _mocklisSymbols = mocklisSymbols;
            _typeParameterNameSubstitutions = new Dictionary<string, string>();
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

        public TypeSyntax ParseTypeName(ITypeSymbol propertyType)
        {
            var x = propertyType.ToMinimalDisplayParts(_semanticModel, _classDeclaration.SpanStart);

            string s = string.Empty;
            foreach (var part in x)
            {
                switch (part.Kind)
                {
                    case SymbolDisplayPartKind.TypeParameterName:
                    {
                        if (part.Symbol.ContainingSymbol is IMethodSymbol methodSymbol && methodSymbol.TypeParameters.Contains(part.Symbol))
                        {
                            s += FindTypeParameterName(part.Symbol.Name);
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

            return F.ParseTypeName(s);
        }

        public NameSyntax ParseName(ITypeSymbol symbol)
        {
            return F.ParseName(symbol.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart));
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

            return ApplyTypeParameters(ParseTypeName(symbol));
        }

        public TypeSyntax ActionMethodMock()
        {
            return ParseTypeName(_mocklisSymbols.ActionMethodMock0);
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

        public TypeSyntax MockMissingException() => ParseTypeName(_mocklisSymbols.MockMissingException);


        public TypeSyntax MockType() => ParseTypeName(_mocklisSymbols.MockType);

        public TypeSyntax ByRef(TypeSyntax tresult)
        {
            return ParseGenericType(_mocklisSymbols.ByRef1, tresult);
        }

        public TypeSyntax TypedMockProvider() => ParseTypeName(_mocklisSymbols.TypedMockProvider);

        public TypeSyntax RuntimeArgumentHandle() => ParseTypeName(_mocklisSymbols.RuntimeArgumentHandle);

        public ParameterSyntax AsParameterSyntax(IParameterSymbol p)
        {
            var syntax = F.Parameter(F.Identifier(p.Name)).WithType(ParseTypeName(p.Type));

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

        private TypeParameterConstraintClauseSyntax CreateConstraintClauseFromTypeParameter(ITypeParameterSymbol typeParameter)
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
                constraints.Add(F.TypeConstraint(ParseTypeName(type)));
            }

            if (typeParameter.HasConstructorConstraint)
            {
                constraints.Add(F.ConstructorConstraint());
            }

            if (constraints.Any())
            {
                var name = FindTypeParameterName(typeParameter.Name);
                return F.TypeParameterConstraintClause(F.IdentifierName(name), F.SeparatedList(constraints));
            }

            return null;
        }

        public TypeParameterConstraintClauseSyntax[] AsConstraintClauses(IEnumerable<ITypeParameterSymbol> typeParameters)
        {
            return typeParameters.Select(CreateConstraintClauseFromTypeParameter).Where(a => a != null).ToArray();
        }

        public ExpressionSyntax WrapByRef(ExpressionSyntax invocation, TypeSyntax returnType)
        {
            var byref = ByRef(returnType);
            var wrap = F.InvocationExpression(F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, byref, F.IdentifierName("Wrap")))
                .WithExpressionsAsArgumentList(invocation);
            return F.RefExpression(wrap);
        }
    }
}
