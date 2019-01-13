// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisTypesForSymbols.cs">
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
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class MocklisTypesForSymbols
    {
        private readonly SemanticModel _semanticModel;
        private readonly ClassDeclarationSyntax _classDeclaration;
        private readonly MocklisSymbols _mocklisSymbols;

        public MocklisTypesForSymbols(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols)
        {
            _semanticModel = semanticModel;
            _classDeclaration = classDeclaration;
            _mocklisSymbols = mocklisSymbols;
            MockMissingException = ParseName(mocklisSymbols.MockMissingException);
            MockType = ParseName(mocklisSymbols.MockType);
            TypedMockProvider = ParseName(mocklisSymbols.TypedMockProvider);
            RuntimeArgumentHandle = ParseName(mocklisSymbols.RuntimeArgumentHandle);
        }

        public TypeSyntax ParseTypeName(ITypeSymbol propertyType)
        {
            return F.ParseTypeName(propertyType.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart));
        }

        public NameSyntax ParseName(ITypeSymbol symbol)
        {
            return F.ParseName(symbol.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart));
        }

        private NameSyntax ParseGenericName(ITypeSymbol symbol, params TypeSyntax[] typeParameters)
        {
            NameSyntax ApplyTypeParameters(NameSyntax nameSyntax)
            {
                if (nameSyntax is GenericNameSyntax genericNameSyntax)
                {
                    return genericNameSyntax.WithTypeArgumentList(F.TypeArgumentList(F.SeparatedList(typeParameters)));
                }

                if (nameSyntax is QualifiedNameSyntax qualifiedNameSyntax)
                {
                    return qualifiedNameSyntax.WithRight((SimpleNameSyntax)ApplyTypeParameters(qualifiedNameSyntax.Right));
                }

                return nameSyntax;
            }

            return ApplyTypeParameters(ParseName(symbol));
        }

        public TypeSyntax ActionMethodMock()
        {
            return ParseTypeName(_mocklisSymbols.ActionMethodMock0);
        }

        public TypeSyntax ActionMethodMock(TypeSyntax tparam)
        {
            return ParseGenericName(_mocklisSymbols.ActionMethodMock1, tparam);
        }

        public TypeSyntax EventMock(TypeSyntax thandler)
        {
            return ParseGenericName(_mocklisSymbols.EventMock1, thandler);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.FuncMethodMock1, tresult);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tparam, TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.FuncMethodMock2, tparam, tresult);
        }

        public TypeSyntax IndexerMock(TypeSyntax tkey, TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.IndexerMock2, tkey, tvalue);
        }

        public TypeSyntax PropertyMock(TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.PropertyMock1, tvalue);
        }

        public TypeSyntax MockMissingException { get; }

        public TypeSyntax MockType { get; }

        public TypeSyntax ByRef(TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.ByRef1, tresult);
        }

        public TypeSyntax TypedMockProvider { get; }

        public TypeSyntax RuntimeArgumentHandle { get; }

        public ArgumentSyntax AsArgumentSyntax(IParameterSymbol p)
        {
            var syntax = F.Argument(F.IdentifierName(p.Name));

            switch (p.RefKind)
            {
                case RefKind.Out:
                {
                    syntax = syntax.WithRefOrOutKeyword(F.Token(SyntaxKind.OutKeyword));
                    break;
                }
                case RefKind.Ref:
                {
                    syntax = syntax.WithRefOrOutKeyword(F.Token(SyntaxKind.RefKeyword));
                    break;
                }
            }

            return syntax;
        }

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

            if (typeParameter.HasValueTypeConstraint)
            {
                constraints.Add(F.ClassOrStructConstraint(SyntaxKind.StructConstraint));
            }

            if (typeParameter.HasUnmanagedTypeConstraint)
            {
                constraints.Add(F.TypeConstraint(F.IdentifierName("unmanaged")));
            }

            foreach (var type in typeParameter.ConstraintTypes)
            {
                constraints.Add(F.TypeConstraint(ParseName(type)));
            }

            if (typeParameter.HasConstructorConstraint)
            {
                constraints.Add(F.ConstructorConstraint());
            }

            if (constraints.Any())
            {
                return F.TypeParameterConstraintClause(F.IdentifierName(typeParameter.Name), F.SeparatedList(constraints));
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
