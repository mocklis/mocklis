// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyEventStepRemoveTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class DummyEventStepRemoveTests
    {
        private void MyEventHandler(object? sender, EventArgs e)
        {
        }

        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void NotThrow()
        {
            _mockMembers.MyEvent.Dummy();
            ((IEvents)_mockMembers).MyEvent -= MyEventHandler;
        }
    }
}
