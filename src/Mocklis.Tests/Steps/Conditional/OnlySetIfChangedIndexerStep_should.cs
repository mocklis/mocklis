// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlySetIfChangedIndexerStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using Mocklis.Tests.Helpers;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class OnlySetIfChangedIndexerStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void only_forward_values_if_changed()
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
        public void only_forward_values_if_comparer_considers_them_changed()
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
        public void not_throw_if_next_step_missing()
        {
            MockMembers.Item.OnlySetIfChanged();
            Sut[0] = "Aha";
        }
    }
}
