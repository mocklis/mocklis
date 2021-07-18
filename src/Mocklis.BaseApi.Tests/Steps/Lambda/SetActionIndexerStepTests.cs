// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetActionIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using System.Linq;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class SetActionIndexerStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.Item.SetAction(null!));
        }

        [Fact]
        public void InvokeActionOnSets()
        {
            string? setValue = null;

            MockMembers.Item.SetAction((i, v) => setValue = v);

            Sut[5] = "Test";

            Assert.Equal("Test", setValue);
        }

        [Fact]
        public void ForwardGets()
        {
            MockMembers.Item.SetAction((i, v) => throw new InvalidOperationException()).RecordAfterGet(out var ledger);

            var _ = Sut[5];

            Assert.Equal(new[] { (5, (string)null!) }, ledger);
            Assert.Equal(new[] { (5, (string)null!) }, ledger.ToArray());
        }
    }
}
