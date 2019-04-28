// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleTypeOrValueTuple.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class SingleTypeOrValueTuple
    {
        private struct NameAndType
        {
            public NameAndType(string tupleSafeName, ITypeSymbol type)
            {
                TupleSafeName = tupleSafeName;
                Type = type;
            }

            public string TupleSafeName { get; }
            public ITypeSymbol Type { get; }
        }

        public bool IsMultiDimensional { get; }

        private NameAndType[] Items { get; }

        public SingleTypeOrValueTuple(IEnumerable<IParameterSymbol> parameters, string mockMemberName = null)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var paramArray = parameters.ToArray();

            int count = paramArray.Length;

            if (count == 0)
            {
                throw new ArgumentException("Parameter list must contain at least one element", nameof(parameters));
            }

            IsMultiDimensional = count != 1;

            Items = new NameAndType[count];

            if (IsMultiDimensional)
            {
                var uniquifier = new Uniquifier();

                for (int i = 0; i < count; i++)
                {
                    string name = paramArray[i].Name;
                    name = name == mockMemberName ? name + "_" : name;
                    if (IsNameValidForPosition(name, i))
                    {
                        name = uniquifier.GetUniqueName(name);
                        Items[i] = new NameAndType(name, paramArray[i].Type);
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    string name = paramArray[i].Name;
                    name = name == mockMemberName ? name + "_" : name;
                    if (!IsNameValidForPosition(name, i))
                    {
                        name = uniquifier.GetUniqueName(name + "_");
                        Items[i] = new NameAndType(name, paramArray[i].Type);
                    }
                }
            }
            else
            {
                string name = paramArray[0].Name;
                name = name == mockMemberName ? name + "_" : name;

                Items[0] = new NameAndType(name, paramArray[0].Type);
            }
        }

        private bool IsNameValidForPosition(string name, int position)
        {
            if (!name.StartsWith("Item"))
            {
                return true;
            }

            string rest = name.Substring(4);
            if (rest == string.Empty)
            {
                return true;
            }

            if (rest[0] == '0')
            {
                return true;
            }

            if (rest == (position + 1).ToString(CultureInfo.InvariantCulture))
            {
                return true;
            }

            return rest.Any(ch => ch < '0' || ch > '9');
        }

        public TypeSyntax BuildTypeSyntax(MocklisTypesForSymbols typesForSymbols)
        {
            return
                IsMultiDimensional
                    ? F.TupleType(F.SeparatedList(Items.Select(a =>
                        F.TupleElement(typesForSymbols.ParseTypeName(a.Type), F.Identifier(a.TupleSafeName)))))
                    : typesForSymbols.ParseTypeName(Items[0].Type);
        }

        public BracketedParameterListSyntax BuildParameterList(MocklisTypesForSymbols typesForSymbols)
        {
            return F.BracketedParameterList(F.SeparatedList(Items.Select(a =>
                F.Parameter(F.Identifier(a.TupleSafeName)).WithType(typesForSymbols.ParseTypeName(a.Type)))));
        }

        public ExpressionSyntax BuildArgumentList()
        {
            return IsMultiDimensional
                ? (ExpressionSyntax)F.TupleExpression(F.SeparatedList(Items
                    .Select(a => F.Argument(F.IdentifierName(a.TupleSafeName))).ToArray()))
                : F.IdentifierName(Items[0].TupleSafeName);
        }
    }
}
