// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceGenerationContext.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;

    #endregion

    public class SourceGenerationContext
    {
        private const string Indentation = "    ";
        private readonly List<string> _constructorStatements = new();
        private readonly StringBuilder _stringBuilder = new();
        private readonly MockSettings _settings;
        private readonly INamedTypeSymbol _classSymbol;
        private readonly bool _nullableAnnotationsEnabled;

        private int _indentationLevel;

        public SourceGenerationContext(MockSettings settings, INamedTypeSymbol classSymbol, bool nullableAnnotationsEnabled)
        {
            _settings = settings;
            _classSymbol = classSymbol;
            _nullableAnnotationsEnabled = nullableAnnotationsEnabled;
        }

        public void IncreaseIndent()
        {
            _indentationLevel++;
        }

        public void DecreaseIndent()
        {
            _indentationLevel--;
        }

        private void AppendIndentation()
        {
            for (int i = 0; i < _indentationLevel; i++)
            {
                _stringBuilder.Append(Indentation);
            }
        }

        public void AppendLine(string text)
        {
            AppendIndentation();
            _stringBuilder.AppendLine(text);
        }

        public void AppendLine()
        {
            _stringBuilder.AppendLine();
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }


        public string ParseTypeName(ITypeSymbol typeSymbol, bool makeNullableIfPossible, Func<string, string>? findTypeParameterName = null)
        {
            var x = typeSymbol.ToDisplayParts(SymbolDisplayFormat.FullyQualifiedFormat);
            // var x = typeSymbol.ToMinimalDisplayParts(_semanticModel, _classDeclaration.SpanStart, SymbolDisplayFormat);

            string s = string.Empty;
            foreach (var part in x)
            {
                switch (part.Kind)
                {
                    case SymbolDisplayPartKind.TypeParameterName:
                    {
                        var partSymbol = part.Symbol;

                        if (partSymbol is { ContainingSymbol: IMethodSymbol methodSymbol } &&
                            methodSymbol.TypeParameters.Contains(partSymbol, SymbolEqualityComparer.Default))
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

            if (makeNullableIfPossible && _nullableAnnotationsEnabled && typeSymbol.IsReferenceType)
            {
                s += "?";
            }

            return s;
        }
    }
}
