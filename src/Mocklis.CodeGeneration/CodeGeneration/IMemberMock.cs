// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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
        void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList);
        void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements);
    }
}
