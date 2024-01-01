// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Substitutions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

#endregion

public sealed class Substitutions : ITypeParameterSubstitutions
{
    public static ITypeParameterSubstitutions Empty { get; } = new EmptyTypeParameterSubstitutions();

    private sealed class EmptyTypeParameterSubstitutions : ITypeParameterSubstitutions
    {
        string ITypeParameterSubstitutions.FindSubstitution(string typeParameterName) => typeParameterName;
    }

    public static ITypeParameterSubstitutions Build(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol)
    {
        if (classSymbol.TypeParameters.Any() && methodSymbol.TypeParameters.Any())
        {
            return new Substitutions(classSymbol, methodSymbol);
        }

        return Empty;
    }

    private readonly Dictionary<string, string> _typeParameterNameSubstitutions;

    private Substitutions(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol)
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
