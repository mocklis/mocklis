// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public sealed class NullMemberMock : IMemberMock, ISyntaxAdder
    {
        public static IMemberMock Instance { get; } = new NullMemberMock();

        private NullMemberMock()
        {
        }

        void ISyntaxAdder.AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
            string interfaceName)
        {
        }

        void ISyntaxAdder.AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, string className, string interfaceName)
        {
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict) => this;
    }
}
