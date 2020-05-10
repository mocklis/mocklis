// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public class ActionMethodStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.SimpleAction.Action((Action)null!));
            Assert.Throws<ArgumentNullException>(() => MockMembers.ActionWithParameter.Action(null!));
        }

        [Fact]
        public void CallActionWithNoParameters()
        {
            bool isCalled = false;
            MockMembers.SimpleAction.Action(() => { isCalled = true; });

            Sut.SimpleAction();

            Assert.True(isCalled);
        }

        [Fact]
        public void CallActionWithParameters()
        {
            int callParameter = 0;
            MockMembers.ActionWithParameter.Action(i => callParameter = i);

            Sut.ActionWithParameter(99);

            Assert.Equal(99, callParameter);
        }
    }
}
