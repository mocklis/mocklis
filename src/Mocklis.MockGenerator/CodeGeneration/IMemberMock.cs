// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

#region Using Directives

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

#endregion

public interface IMemberMock
{
    void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements, NameSyntax interfaceNameSyntax, string className, string interfaceName);

    void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol);
}
