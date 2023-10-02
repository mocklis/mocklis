// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullMemberMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.MockGenerator.CodeGeneration;

    #endregion

    public sealed class NullMemberMock : IMemberMock, ISyntaxAdder
    {
        public static IMemberMock Instance { get; } = new NullMemberMock();

        private NullMemberMock()
        {
        }

        void ISyntaxAdder.AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettingns,
            IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
            string interfaceName)
        {
        }

        void ISyntaxAdder.AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
            List<StatementSyntax> constructorStatements, string className, string interfaceName)
        {
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols) => this;

        public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
        {
        }
    }
}
