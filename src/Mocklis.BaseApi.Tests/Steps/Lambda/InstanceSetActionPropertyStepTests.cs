// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceSetActionPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class InstanceSetActionPropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.StringProperty.InstanceSetAction(null!));
        }

        [Fact]
        public void InvokeActionOnSets()
        {
            object? callInstance = null;
            string? setValue = null;

            MockMembers.StringProperty.InstanceSetAction((obj, a) =>
            {
                callInstance = obj;
                setValue = a;
            });

            Sut.StringProperty = "Test";

            Assert.Same(Sut, callInstance);
            Assert.Equal("Test", setValue);
        }

        [Fact]
        public void ForwardGets()
        {
            MockMembers.StringProperty.InstanceSetAction((o, v) => throw new InvalidOperationException()).RecordAfterGet(out var ledger);

            var _ = Sut.StringProperty;

            Assert.Equal(new[] { (string)null! }, ledger);
        }
    }
}
