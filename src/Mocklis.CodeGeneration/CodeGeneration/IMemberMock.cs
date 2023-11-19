// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
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
        ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict);
    }

    public interface ISyntaxAdder
    {
        void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList);
        void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements);
    }
}
