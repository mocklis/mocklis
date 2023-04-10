// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public interface IMemberMock
    {
        void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict);
        void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict);
    }
}
