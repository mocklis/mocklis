// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public sealed class NullMemberMock : IMemberMock
    {
        public static IMemberMock Instance { get; } = new NullMemberMock();

        private NullMemberMock()
        {
        }

        void IMemberMock.AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
        {
        }

        void IMemberMock.AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
        {
        }
    }
}
