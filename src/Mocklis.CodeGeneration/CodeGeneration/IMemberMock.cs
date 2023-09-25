// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration;

#region Using Directives

using Microsoft.CodeAnalysis;

#endregion

public interface IMemberMock
{
    ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols);
    void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol);
}
