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
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class SingleTypeOrValueTuple : IReadOnlyList<SingleTypeOrValueTuple.Entry>
    {
        public struct Entry
        {
            public Entry(string originalName, TypeSyntax type, bool isReturnValue, string tupleSafeName)
            {
                OriginalName = originalName;
                Type = type;
                IsReturnValue = isReturnValue;
                TupleSafeName = tupleSafeName;
            }

            public string OriginalName { get; }
            public TypeSyntax Type { get; }
            public bool IsReturnValue { get; }
            public string TupleSafeName { get; }
        }

        private Entry[] Entries { get; }

        public SingleTypeOrValueTuple(IEnumerable<Entry> entries)
        {
            Entries = entries?.ToArray() ?? Array.Empty<Entry>();
        }

        public IEnumerator<Entry> GetEnumerator() => Entries.OfType<Entry>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => Entries.Length;

        public Entry this[int index] => Entries[index];

        public bool IsMultiDimensional => Count > 1;

        public TypeSyntax? BuildTypeSyntax()
        {
            if (Count == 0)
            {
                return null;
            }

            return IsMultiDimensional
                ? F.TupleType(F.SeparatedList(this.Select(a => F.TupleElement(a.Type, F.Identifier(a.TupleSafeName)))))
                : this[0].Type;
        }

        public BracketedParameterListSyntax? BuildParameterList()
        {
            if (Count == 0)
            {
                return null;
            }

            return F.BracketedParameterList(F.SeparatedList(this.Select(a => F.Parameter(F.Identifier(a.TupleSafeName)).WithType(a.Type))));
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
    }
}
