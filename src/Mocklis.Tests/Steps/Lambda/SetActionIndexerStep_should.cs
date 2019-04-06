// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetActionIndexerStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class SetActionIndexerStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.Item.SetAction(null));
        }

        [Fact]
        public void invokeAction_on_sets()
        {
            string setValue = null;

            MockMembers.Item.SetAction((i, v) => setValue = v);

            Sut[5] = "Test";

            Assert.Equal("Test", setValue);
        }

        [Fact]
        public void forward_gets()
        {
            MockMembers.Item.SetAction((i, v) => throw new InvalidOperationException()).RecordAfterGet(out var ledger, (i, v) => (i, v)).Dummy();

            var _ = Sut[5];

            Assert.Equal(new[] { (5, (string)null) }, ledger);
        }
    }
}
