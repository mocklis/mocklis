// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISyntaxAdder.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public interface ISyntaxAdder
    {
        void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettingns, IList<MemberDeclarationSyntax> declarationList,
            NameSyntax interfaceNameSyntax, string className, string interfaceName);
        void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
            List<StatementSyntax> constructorStatements, string className, string interfaceName);
    }
}
