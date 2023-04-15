// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullMemberMock.cs">
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

    public sealed class NullMemberMock : IMemberMock, ISyntaxAdder
    {
        public static IMemberMock Instance { get; } = new NullMemberMock();

        private NullMemberMock()
        {
        }

        void ISyntaxAdder.AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
        }

        void ISyntaxAdder.AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
        }

        public ISyntaxAdder GetSyntaxAdder() => this;
    }
}
