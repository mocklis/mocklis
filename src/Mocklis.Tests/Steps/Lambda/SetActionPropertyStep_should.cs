// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetActionPropertyStep_should.cs">
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

    public class SetActionPropertyStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.StringProperty.SetAction(null));
        }

        [Fact]
        public void invokeAction_on_sets()
        {
            string setValue = null;

            MockMembers.StringProperty.SetAction(a => setValue = a);

            Sut.StringProperty = "Test";

            Assert.Equal("Test", setValue);
        }

        [Fact]
        public void forward_gets()
        {
            MockMembers.StringProperty.SetAction(a => throw new InvalidOperationException()).RecordAfterGet(out var ledger, v => v).Dummy();

            var _ = Sut.StringProperty;

            Assert.Equal(new[] { (string)null }, ledger);
        }
    }
}
