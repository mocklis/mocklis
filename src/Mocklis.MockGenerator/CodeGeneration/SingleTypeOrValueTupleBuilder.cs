// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleTypeOrValueTupleBuilder.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.MockGenerator.CodeGeneration.Compatibility;

    #endregion

    public sealed class SingleTypeOrValueTupleBuilder
    {
        private struct BuilderEntry
        {
            public BuilderEntry(string originalName, TypeSyntax type, bool isReturnValue)
            {
                OriginalName = originalName;
                Type = type;
                IsReturnValue = isReturnValue;
            }

            public string OriginalName { get; }
            public TypeSyntax Type { get; }
            public bool IsReturnValue { get; }
        }

        private MocklisTypesForSymbols TypesForSymbols { get; }

        private List<BuilderEntry> Items { get; } = new List<BuilderEntry>();

        public SingleTypeOrValueTupleBuilder(MocklisTypesForSymbols typesForSymbols)
        {
            TypesForSymbols = typesForSymbols ?? throw new ArgumentNullException(nameof(typesForSymbols));
        }

        public void AddParameter(IParameterSymbol parameter)
        {
            var x = TypesForSymbols.ParseTypeName(parameter.Type, parameter.NullableOrOblivious());
            Items.Add(new BuilderEntry(parameter.Name, x, false));
        }

        public void AddReturnValue(ITypeSymbol returnType, bool nullable, Func<string, string> findTypeParameterName)
        {
            Items.Add(new BuilderEntry("returnValue", TypesForSymbols.ParseTypeName(returnType, nullable, findTypeParameterName), true));
        }

        public SingleTypeOrValueTuple Build(string? mockMemberName = null)
        {
            mockMemberName ??= string.Empty;

            int count = Items.Count;

            SingleTypeOrValueTuple.Entry[] entries = new SingleTypeOrValueTuple.Entry[Items.Count];

            if (count > 0)
            {
                if (count > 1)
                {
                    var uniquifier = new Uniquifier(new[] { mockMemberName });

                    for (int i = 0; i < count; i++)
                    {
                        var item = Items[i];
                        string name = item.OriginalName;
                        name = name == mockMemberName ? name + "_" : name;
                        if (IsNameValidForPosition(name, i))
                        {
                            entries[i] = new SingleTypeOrValueTuple.Entry(
                                item.OriginalName,
                                item.Type,
                                item.IsReturnValue,
                                uniquifier.GetUniqueName(name));
                        }
                    }

                    for (int i = 0; i < count; i++)
                    {
                        var item = Items[i];
                        string name = item.OriginalName;
                        name = name == mockMemberName ? name + "_" : name;
                        if (!IsNameValidForPosition(name, i))
                        {
                            entries[i] = new SingleTypeOrValueTuple.Entry(
                                item.OriginalName,
                                item.Type,
                                item.IsReturnValue,
                                uniquifier.GetUniqueName(name + "_"));
                        }
                    }
                }
                else
                {
                    var item = Items[0];
                    string name = item.OriginalName;
                    name = name == mockMemberName ? name + "_" : name;
                    entries[0] = new SingleTypeOrValueTuple.Entry(item.OriginalName, item.Type, item.IsReturnValue, name);
                }
            }

            return new SingleTypeOrValueTuple(entries);
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
    }
}
