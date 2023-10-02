// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceGenerationContext.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.MockGenerator.CodeGeneration;

    #endregion

    public class SourceGenerationContext
    {
        private const string Indentation = "    ";
        private readonly List<string> _constructorStatements = new();
        private readonly StringBuilder _stringBuilder = new();
        private readonly StringBuilder _lineBuilder = new();
        private readonly INamedTypeSymbol _classSymbol;
        private readonly bool _nullableAnnotationsEnabled;

        private int _indentationLevel;
        private bool _addSeparator;

        private readonly string _strictness;

        public IReadOnlyCollection<string> ConstructorStatements => _constructorStatements;

        public SourceGenerationContext(MockSettings settings, INamedTypeSymbol classSymbol, bool nullableAnnotationsEnabled)
        {
            _classSymbol = classSymbol;
            _nullableAnnotationsEnabled = nullableAnnotationsEnabled;

            _strictness = settings.VeryStrict ? "global::Mocklis.Core.Strictness.VeryStrict" :
                (settings.Strict ? "global::Mocklis.Core.Strictness.Strict" : "global::Mocklis.Core.Strictness.Lenient");
        }

        public void IncreaseIndent()
        {
            _indentationLevel++;
            _addSeparator = false;
        }

        public void DecreaseIndent()
        {
            _indentationLevel--;
            _addSeparator = false;
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
            if (_addSeparator)
            {
                _stringBuilder.AppendLine();
                _addSeparator = false;
            }

            _lineBuilder.Append(text);
            if (_lineBuilder.Length > 0)
            {
                AppendIndentation();
                _stringBuilder.AppendLine(_lineBuilder.ToString());
                _lineBuilder.Clear();
            }
        }

        public void AppendSeparator()
        {
            _addSeparator = true;
        }

        public void Append(string text)
        {
            _lineBuilder.Append(text);
        }

        public void AppendLine()
        {
            if (_addSeparator)
            {
                _stringBuilder.AppendLine();
                _addSeparator = false;
            }
            if (_lineBuilder.Length > 0)
            {
                AppendIndentation();
                _stringBuilder.AppendLine(_lineBuilder.ToString());
                _lineBuilder.Clear();
            }
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        public void AddConstructorStatement(string mockPropertyType, string memberMockName, string interfaceName, string symbolName)
        {
            _constructorStatements.Add($"this.{memberMockName} = new {mockPropertyType}(this, \"{_classSymbol.Name}\", \"{interfaceName}\", \"{symbolName}\", \"{memberMockName}\", {_strictness});");
        }

        public string TypedMockCreation(string mockPropertyType, string memberMockName, string interfaceName, string symbolName)
        {
            return $"keyString => new {mockPropertyType}(this, \"{_classSymbol.Name}\", \"{interfaceName}\", \"{symbolName}\" + keyString, \"{memberMockName}\" + keyString, {_strictness})";
        }

        public void AppendThrow(string mockType, string memberMockName, string interfaceName, string symbolName)
        {
            AppendLine($"throw new global::Mocklis.Core.MockMissingException(global::Mocklis.Core.MockType.{mockType}, \"{_classSymbol.Name}\", \"{interfaceName}\", \"{symbolName}\", \"{memberMockName}\");");
        }

        public string ParseTypeName(ITypeSymbol typeSymbol, bool makeNullableIfPossible, ITypeParameterSubstitutions substitutions)
        {
            var x = typeSymbol.ToDisplayParts(SymbolDisplayFormat.FullyQualifiedFormat);

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
                                s += substitutions.FindSubstitution(partSymbol.Name);
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

        public string? BuildTupleType(SingleTypeOrValueTuple tuple, ITypeParameterSubstitutions substitutions)
        {
            string BuildEntry(SingleTypeOrValueTuple.Entry entry)
            {
                return ParseTypeName(entry.TypeSymbol, entry.IsNullable, substitutions);
            }

            return tuple.Count switch
            {
                0 => null,
                1 => BuildEntry(tuple[0]),
                _ => $"({string.Join(", ", tuple.Select(t => $"{BuildEntry(t)} {t.TupleSafeName}"))})"
            };
            
        }

        public string BuildParameterList(IEnumerable<IParameterSymbol> parameters, ITypeParameterSubstitutions substitutions)
        {
            var args = parameters.Select(p =>
            {
                var syntax = $"{ParseTypeName(p.Type, p.NullableOrOblivious(), substitutions)} {p.Name}";

                switch (p.RefKind)
                {
                    case RefKind.In:
                    {
                        return $"in {syntax}";
                    }

                    case RefKind.Out:
                    {
                        return $"out {syntax}";
                    }

                    case RefKind.Ref:
                    {
                        return $"ref {syntax}";
                    }
                }

                return syntax;
            });

            return string.Join(", ", args);
        }

        public string BuildArgumentList(IEnumerable<IParameterSymbol> parameters)
        {
            var args = parameters.Select(p =>
            {
                switch (p.RefKind)
                {
                    case RefKind.In:
                    {
                        return $"in {p.Name}";
                    }

                    case RefKind.Out:
                    {
                        return $"out {p.Name}";
                    }

                    case RefKind.Ref:
                    {
                        return $"ref {p.Name}";
                    }
                }

                return $"{p.Name}";
            });

            return string.Join(", ", args);
        }

        public string BuildConstraintClauses(IReadOnlyCollection<ITypeParameterSymbol> typeParameters, ITypeParameterSubstitutions substitutions, bool classConstraintInNullableContextOnly)
        {
            StringBuilder result = new StringBuilder();

            void CreateConstraintClauseFromTypeParameter(ITypeParameterSymbol typeParameter)
            {
                var constraints = new List<string>();

                if (typeParameter.HasReferenceTypeConstraint)
                {
                    if (classConstraintInNullableContextOnly)
                    {
                        if (_nullableAnnotationsEnabled)
                        {
                            constraints.Add("class");
                        }
                    }
                    else
                    {
                        constraints.Add("class");
                    }
                }

                if (!classConstraintInNullableContextOnly)
                {
                    // Note that 'unmanaged' is a type of valuetype constraint.
                    if (typeParameter.HasUnmanagedTypeConstraint)
                    {
                        constraints.Add("unmanaged");
                    }
                    else if (typeParameter.HasValueTypeConstraint)
                    {
                        constraints.Add("struct");
                    }

                    foreach (var type in typeParameter.ConstraintTypes)
                    {
                        if (!type.IsValueType)
                        {
                            // TODO: Original code didn't have substitutions here...
                            constraints.Add(ParseTypeName(type, false, substitutions));
                        }
                    }

                    if (typeParameter.HasConstructorConstraint)
                    {
                        constraints.Add("new()");
                    }

                    if (typeParameter.HasNotNullConstraint && _nullableAnnotationsEnabled)
                    {
                        constraints.Add("notnull");
                    }
                }

                if (constraints.Any())
                {
                    var name = substitutions.FindSubstitution(typeParameter.Name);
                    result.Append($" where {name} : {string.Join(", ", constraints)}");
                }
            }

            foreach (var parameter in typeParameters)
            {
                CreateConstraintClauseFromTypeParameter(parameter);
            }

            return result.ToString();
        }

        public (string returnType, string returnTypeWithoutReadonly) FindReturnTypes(IMethodSymbol symbol, ITypeParameterSubstitutions substitutions)
        {
            if (symbol.ReturnsVoid)
            {
                return ("void", "void");
            }

            var tmp = ParseTypeName(symbol.ReturnType, symbol.ReturnTypeIsNullableOrOblivious(), substitutions);

            if (symbol.ReturnsByRef)
            {
                return ("ref " + tmp, "ref " + tmp);
            }

            if (symbol.ReturnsByRefReadonly)
            {
                return ("ref readonly " + tmp, "ref " + tmp);
            }

            return (tmp, tmp);
        }

        public (string returnType, string returnTypeWithoutReadonly) FindPropertyTypes(IPropertySymbol Symbol)
        {
            var tmp = ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious(), Substitutions.Empty);

            if (Symbol.ReturnsByRef)
            {
                return ("ref " + tmp, "ref " + tmp);
            }

            if (Symbol.ReturnsByRefReadonly)
            {
                return ("ref readonly " + tmp, "ref " + tmp);
            }

            return (tmp, tmp);
        }
    }
}
