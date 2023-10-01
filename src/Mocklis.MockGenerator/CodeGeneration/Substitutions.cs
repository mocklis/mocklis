// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Substitutions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Mocklis.CodeGeneration.UniqueNames;

    #endregion

    public interface ITypeParameterSubstitutions
    {
        string FindSubstitution(string typeParameterName);

        public static ITypeParameterSubstitutions Empty { get; } = new EmptyTypeParameterSubstitutions();

        public static ITypeParameterSubstitutions Build(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol)
        {
            if (classSymbol.TypeParameters.Any() && methodSymbol.TypeParameters.Any())
            {
                return new Substitutions(classSymbol, methodSymbol);
            }

            return Empty;
        }

        private sealed class EmptyTypeParameterSubstitutions : ITypeParameterSubstitutions
        {
            string ITypeParameterSubstitutions.FindSubstitution(string typeParameterName) => typeParameterName;
        }

        private sealed class Substitutions : ITypeParameterSubstitutions
        {
            private readonly Dictionary<string, string> _typeParameterNameSubstitutions;

            public Substitutions(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol)
            {
                _typeParameterNameSubstitutions = new Dictionary<string, string>();
                Uniquifier t = new Uniquifier(classSymbol.TypeParameters.Select(tp => tp.Name));

                foreach (var methodTypeParameter in methodSymbol.TypeParameters)
                {
                    string uniqueName = t.GetUniqueName(methodTypeParameter.Name);
                    _typeParameterNameSubstitutions[methodTypeParameter.Name] = uniqueName;
                }
            }

            string ITypeParameterSubstitutions.FindSubstitution(string typeParameterName)
            {
                return _typeParameterNameSubstitutions.TryGetValue(typeParameterName, out var substitution) ? substitution : typeParameterName;
            }
        }
    }
}