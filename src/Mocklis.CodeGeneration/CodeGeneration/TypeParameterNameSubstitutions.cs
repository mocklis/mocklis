// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeParameterNameSubstitutions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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

    public class TypeParameterNameSubstitutions
    {
        private readonly Dictionary<string, string> _typeParameterNameTranslations = new Dictionary<string, string>();

        public TypeParameterNameSubstitutions(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol)
        {
            Uniquifier t = new Uniquifier(classSymbol.TypeParameters.Select(tp => tp.Name));

            foreach (var methodTypeParameter in methodSymbol.TypeParameters)
            {
                string uniqueName = t.GetUniqueName(methodTypeParameter.Name);
                _typeParameterNameTranslations[methodTypeParameter.Name] = uniqueName;
            }
        }

        public string GetName(string typeParameterName)
        {
            if (_typeParameterNameTranslations.ContainsKey(typeParameterName))
            {
                return _typeParameterNameTranslations[typeParameterName];
            }

            return typeParameterName;
        }
    }
}
