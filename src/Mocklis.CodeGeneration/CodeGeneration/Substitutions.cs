// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Substitutions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Mocklis.CodeGeneration.UniqueNames;

    public class Substitutions
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

        public string FindTypeParameterName(string typeParameterName)
        {
            return _typeParameterNameSubstitutions.TryGetValue(typeParameterName, out var substitution) ? substitution : typeParameterName;
        }
    }
}
