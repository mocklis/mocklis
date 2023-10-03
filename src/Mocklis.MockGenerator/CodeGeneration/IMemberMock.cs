// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

using System.Collections.Generic;

#region Using Directives

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

public interface IMemberMock
{
    void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList, List<StatementSyntax> constructorStatements, NameSyntax interfaceNameSyntax,  string className, string interfaceName);
    void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol);
}
