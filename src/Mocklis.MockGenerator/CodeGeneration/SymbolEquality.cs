// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SymbolEquality.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

#endregion

public static class SymbolEquality
{
    public static IEqualityComparer<INamedTypeSymbol> ForClass { get; } = new ClassEquality();

    public static IEqualityComparer<INamedTypeSymbol> ForInterface { get; } = new InterfaceEquality();

    public static IEqualityComparer<IEventSymbol> ForEvent { get; } = new EventEquality();

    public static IEqualityComparer<IPropertySymbol> ForProperty { get; } = new PropertyEquality();

    public static IEqualityComparer<IMethodSymbol> ForMethod { get; } = new MethodEquality();

    private static bool SameNamespace(INamespaceSymbol x, INamespaceSymbol y)
    {
        if (x.IsGlobalNamespace && y.IsGlobalNamespace) return true;

        if (x.IsGlobalNamespace) return false;
        if (y.IsGlobalNamespace) return false;

        return (x.Name == y.Name && SameNamespace(x.ContainingNamespace, y.ContainingNamespace));
    }

    private static bool SameArray<T>(IReadOnlyList<T> x, IReadOnlyList<T> y, Func<T, T, bool> comparer)
    {
        if (x.Count != y.Count) return false;

        for (var i = 0; i < x.Count; i++)
        {
            if (!comparer(x[i], y[i])) return false;
        }

        return true;
    }

    private static bool SameConstructor(IMethodSymbol x, IMethodSymbol y)
    {
        return SameArray(x.Parameters, y.Parameters, SameParameter);
    }

    private static bool SameParameter(IParameterSymbol x, IParameterSymbol y)
    {
        if (x.Name != y.Name) return false;
        if (!SameType(x.Type, y.Type)) return false;

        return true;
    }

    private static bool SameType(ITypeSymbol x, ITypeSymbol y)
    {
        if (x.Name != y.Name) return false;
        if (!SameNamespace(x.ContainingNamespace, y.ContainingNamespace)) return false;

        return true;
    }

    private static bool SameTypeParameter(ITypeParameterSymbol x, ITypeParameterSymbol y)
    {
        if (x.Name != y.Name) return false;

        // Constraints?

        return true;
    }

    private class ClassEquality : IEqualityComparer<INamedTypeSymbol>
    {
        public bool Equals(INamedTypeSymbol x, INamedTypeSymbol y)
        {
            // same name
            if (x.Name != y.Name) return false;

            // live in same namespace
            if (!SameNamespace(x.ContainingNamespace, y.ContainingNamespace)) return false;

            // have the same constructors
            var xConst = x.BaseType?.Constructors.Where(c => !c.IsStatic && !c.IsVararg && c.CanBeAccessedBySubClass()).ToArray() ??
                         Array.Empty<IMethodSymbol>();
            var yConst = y.BaseType?.Constructors.Where(c => !c.IsStatic && !c.IsVararg && c.CanBeAccessedBySubClass()).ToArray() ??
                         Array.Empty<IMethodSymbol>();
            if (!SameArray(xConst, yConst, SameConstructor)) return false;

            // have the same type parameters
            if (!SameArray(x.TypeParameters, y.TypeParameters, SameTypeParameter)) return false;

            return true;
        }

        public int GetHashCode(INamedTypeSymbol obj)
        {
            // Just something simple - it won't really be used.
            return obj.Name.GetHashCode();
        }
    }

    private class InterfaceEquality : IEqualityComparer<INamedTypeSymbol>
    {
        public bool Equals(INamedTypeSymbol x, INamedTypeSymbol y)
        {
            // same name
            if (x.Name != y.Name) return false;

            // live in same namespace
            if (!SameNamespace(x.ContainingNamespace, y.ContainingNamespace)) return false;

            // have the same type parameters
            if (!SameArray(x.TypeParameters, y.TypeParameters, SameTypeParameter)) return false;

            return true;
        }

        public int GetHashCode(INamedTypeSymbol obj)
        {
            // Just something simple - it won't really be used.
            return obj.Name.GetHashCode();
        }
    }

    private class EventEquality : IEqualityComparer<IEventSymbol>
    {
        public bool Equals(IEventSymbol x, IEventSymbol y)
        {
            // same name
            if (x.Name != y.Name) return false;

            // Same type
            if (!SameType(x.Type, y.Type)) return false;

            return true;
        }

        public int GetHashCode(IEventSymbol obj)
        {
            // Just something simple - it won't really be used.
            return obj.Name.GetHashCode();
        }
    }

    // Note that this is used both for indexers and properties
    private class PropertyEquality : IEqualityComparer<IPropertySymbol>
    {
        public bool Equals(IPropertySymbol x, IPropertySymbol y)
        {
            if (x.IsReadOnly != y.IsReadOnly) return false;

            if (x.IsWriteOnly != y.IsWriteOnly) return false;

            // same name
            if (x.Name != y.Name) return false;

            // Have the same type
            if (!SameType(x.Type, y.Type)) return false;

            // same parameters
            if (!SameArray(x.Parameters, y.Parameters, SameParameter)) return false;

            return true;
        }

        public int GetHashCode(IPropertySymbol obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    private class MethodEquality : IEqualityComparer<IMethodSymbol>
    {
        public bool Equals(IMethodSymbol x, IMethodSymbol y)
        {
            // same name
            if (x.Name != y.Name) return false;

            // Have the same return type
            if (!SameType(x.ReturnType, y.ReturnType)) return false;

            // same parameters
            if (!SameArray(x.Parameters, y.Parameters, SameParameter)) return false;

            return true;
        }

        public int GetHashCode(IMethodSymbol obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
