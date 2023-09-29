// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleTypeOrValueTuple.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class SingleTypeOrValueTuple : IReadOnlyList<SingleTypeOrValueTuple.Entry>
    {
        public readonly struct Entry
        {
            public Entry(string originalName, ITypeSymbol typeSymbol, bool isNullable, bool isReturnValue, string tupleSafeName)
            {
                OriginalName = originalName;
                TypeSymbol = typeSymbol;
                IsNullable = isNullable;
                IsReturnValue = isReturnValue;
                TupleSafeName = tupleSafeName;
            }

            public string OriginalName { get; }
            public ITypeSymbol TypeSymbol { get; }
            public bool IsNullable { get; }
            public bool IsReturnValue { get; }
            public string TupleSafeName { get; }

            public TypeSyntax CreateType(MocklisTypesForSymbols typesForSymbols, Func<string, string>? typeParameterNameSubstitutions)
            {
                return typesForSymbols.ParseTypeName(TypeSymbol, IsNullable, typeParameterNameSubstitutions);
            }
        }

        private Entry[] Entries { get; }

        public SingleTypeOrValueTuple(IEnumerable<Entry> entries)
        {
            Entries = entries.ToArray();
        }

        public IEnumerator<Entry> GetEnumerator() => Entries.OfType<Entry>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => Entries.Length;

        public Entry this[int index] => Entries[index];

        public bool IsMultiDimensional => Count > 1;

        public TypeSyntax? BuildTypeSyntax(MocklisTypesForSymbols typesForSymbols, Func<string, string>? typeParameterNameSubstitutions)
        {
            if (Count == 0)
            {
                return null;
            }

            return IsMultiDimensional
                ? F.TupleType(F.SeparatedList(this.Select(a => F.TupleElement(a.CreateType(typesForSymbols, typeParameterNameSubstitutions), F.Identifier(a.TupleSafeName)))))
                : this[0].CreateType(typesForSymbols, typeParameterNameSubstitutions);
        }

        public BracketedParameterListSyntax BuildParameterList(MocklisTypesForSymbols typesForSymbols, Func<string, string>? typeParameterNameSubstitutions)
        {
            return F.BracketedParameterList(F.SeparatedList(this.Select(a => F.Parameter(F.Identifier(a.TupleSafeName)).WithType(a.CreateType(typesForSymbols, typeParameterNameSubstitutions)))));
        }

        public ExpressionSyntax? BuildArgumentList()
        {
            if (Count == 0)
            {
                return null;
            }

            return IsMultiDimensional
                ? (ExpressionSyntax)F.TupleExpression(F.SeparatedList(this.Select(a => F.Argument(F.IdentifierName(a.TupleSafeName))).ToArray()))
                : F.IdentifierName(this[0].TupleSafeName);
        }

        public ExpressionSyntax? BuildArgumentListWithOriginalNames()
        {
            if (Count == 0)
            {
                return null;
            }

            return IsMultiDimensional
                ? (ExpressionSyntax)F.TupleExpression(F.SeparatedList(this.Select(a => F.Argument(F.IdentifierName(a.OriginalName))).ToArray()))
                : F.IdentifierName(this[0].OriginalName);
        }

        public string BuildArgumentListAsString()
        {
            if (Count == 0)
            {
                return string.Empty;
            }

            if (IsMultiDimensional)
            {
                return $"({string.Join(", ", this.Select(a => a.OriginalName))})";
            }

            return this[0].OriginalName;
        }
    }
}
