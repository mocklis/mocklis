// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtractedInterfaceInformation.cs">
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
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

public sealed class ExtractedInterfaceInformation : IEquatable<ExtractedInterfaceInformation>
{
    public bool Equals(ExtractedInterfaceInformation? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        var mc = _memberMocks.Length;
        if (mc != other._memberMocks.Length) return false;
        for (int i = 0; i < mc; i++)
        {
            if (!_memberMocks[i].Equals(other._memberMocks[i]))
            {
                return false;
            }
        }

        return SymbolEquality.ForInterface.Equals(InterfaceSymbol, other.InterfaceSymbol);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ExtractedInterfaceInformation other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = 0;
            foreach (var m in _memberMocks)
            {
                hashCode = (hashCode * 397) ^ m.GetHashCode();
            }

            hashCode = (hashCode * 397) ^ SymbolEquality.ForInterface.GetHashCode(InterfaceSymbol);
            return hashCode;
        }
    }

    public static bool operator ==(ExtractedInterfaceInformation? left, ExtractedInterfaceInformation? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ExtractedInterfaceInformation? left, ExtractedInterfaceInformation? right)
    {
        return !Equals(left, right);
    }

    public INamedTypeSymbol InterfaceSymbol { get; }
    private readonly IMemberMock[] _memberMocks;

    public ExtractedInterfaceInformation(INamedTypeSymbol interfaceSymbol, IReadOnlyCollection<IMemberMock> memberMocks)
    {
        InterfaceSymbol = interfaceSymbol;
        _memberMocks = memberMocks.ToArray();
    }

    public void GenerateMembers(MocklisTypesForSymbols typesForSymbols, List<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements, INamedTypeSymbol classSymbol)
    {
        var interfaceNameSyntax = typesForSymbols.ParseName(InterfaceSymbol);
        foreach (var memberMock in _memberMocks)
        {
            memberMock.AddSyntax(typesForSymbols, declarationList, constructorStatements, interfaceNameSyntax, classSymbol.Name,
                InterfaceSymbol.Name);
        }
    }

    public void AddSourceForMembers(SourceGenerationContext ctx)
    {
        foreach (var memberMock in _memberMocks)
        {
            memberMock.AddSource(ctx, InterfaceSymbol);
        }
    }
}
