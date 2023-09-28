// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleTypeOrValueTupleBuilder.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.CodeGeneration.UniqueNames;

    #endregion

    public sealed class SingleTypeOrValueTupleBuilder
    {
        private readonly struct BuilderEntry
        {
            public BuilderEntry(string originalName, ITypeSymbol typeSymbol, bool isNullable, bool isReturnValue)
            {
                OriginalName = originalName;
                TypeSymbol = typeSymbol;
                IsNullable = isNullable;
                IsReturnValue = isReturnValue;
            }

            public string OriginalName { get; }
            public ITypeSymbol TypeSymbol { get; }
            public bool IsNullable { get; }
            public bool IsReturnValue { get; }
        }

        private List<BuilderEntry> Items { get; } = new();

        public void AddParameter(IParameterSymbol parameter)
        {
            Items.Add(new BuilderEntry(parameter.Name, parameter.Type, parameter.NullableOrOblivious(), false));
        }

        public void AddReturnValue(ITypeSymbol returnType, bool nullable)
        {
            Items.Add(new BuilderEntry("returnValue", returnType, nullable, true));
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
                                item.TypeSymbol,
                                item.IsNullable,
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
                                item.TypeSymbol,
                                item.IsNullable,
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
                    entries[0] = new SingleTypeOrValueTuple.Entry(item.OriginalName, item.TypeSymbol, item.IsNullable, item.IsReturnValue, name);
                }
            }

            return new SingleTypeOrValueTuple(entries);
        }

        private static bool IsNameValidForPosition(string name, int position)
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
