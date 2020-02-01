// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceSetActionIndexerStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public class InstanceSetActionIndexerStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.Item.InstanceSetAction(null!));
        }

        [Fact]
        public void invokeAction_on_sets()
        {
            object? callInstance = null;
            string? setValue = null;

            MockMembers.Item.InstanceSetAction((obj, i, v) =>
            {
                callInstance = obj;
                setValue = v;
            });

            Sut[5] = "Test";

            Assert.Same(Sut, callInstance);
            Assert.Equal("Test", setValue);
        }

        [Fact]
        public void forward_gets()
        {
            MockMembers.Item.InstanceSetAction((o, i, v) => throw new InvalidOperationException()).RecordAfterGet(out var ledger);

            var _ = Sut[5];

            Assert.Equal(new[] { (5, (string)null!) }, ledger);
        }
    }
}
