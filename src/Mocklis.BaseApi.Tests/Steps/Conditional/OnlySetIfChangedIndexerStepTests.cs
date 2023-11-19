// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlySetIfChangedIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class OnlySetIfChangedIndexerStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void OnlyForwardValuesIfChanged()
        {
            MockMembers.Item.OnlySetIfChanged().RecordBeforeSet(out var ledger, (i, v) => v).StoredAsDictionary();

            Sut[0] = "Alpha";
            Sut[0] = "Beta";
            Sut[0] = "Beta";
            Sut[0] = "Alpha";
            Sut[0] = "Alpha";
            Sut[0] = "Gamma";

            Assert.Equal(new[] { "Alpha", "Beta", "Alpha", "Gamma" }, ledger);
        }

        [Fact]
        public void OnlyForwardValuesIfComparerConsidersThemChanged()
        {
            MockMembers.Item.OnlySetIfChanged(new StringLengthComparer()).RecordBeforeSet(out var ledger, (i, v) => v).StoredAsDictionary();

            Sut[0] = "One";
            Sut[0] = "Two";
            Sut[0] = "Three";
            Sut[0] = "Four";
            Sut[0] = "Five";
            Sut[0] = "Six";

            Assert.Equal(new[] { "One", "Three", "Four", "Six" }, ledger);
        }

        [Fact]
        public void NotThrowIfNextStepMissing()
        {
            MockMembers.Item.OnlySetIfChanged();
            Sut[0] = "Aha";
        }
    }
}
