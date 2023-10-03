// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtractedInterfaceInformation.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ExtractedInterfaceInformation
{
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
            memberMock.AddSyntax(typesForSymbols, declarationList, constructorStatements, interfaceNameSyntax, classSymbol.Name, InterfaceSymbol.Name);
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
