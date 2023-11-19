// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfRemoveEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class IfRemoveEventStepTests
    {
        private void MyEventHandler(object? sender, EventArgs e)
        {
        }

        private IEvents Sut { get; }
        private IReadOnlyList<EventHandler?> Adds { get; }
        private IReadOnlyList<EventHandler?> Removes { get; }

        public IfRemoveEventStepTests()
        {
            var mockMembers = new MockMembers();

            IReadOnlyList<EventHandler?>? adds = null;
            IReadOnlyList<EventHandler?>? removes = null;

            mockMembers.MyEvent
                .IfRemove(i => i
                    .RecordBeforeAdd(out adds)
                    .RecordBeforeRemove(out removes)
                    .Join(i.ElseBranch));

            Adds = adds!;
            Removes = removes!;
            Sut = mockMembers;
        }

        [Fact]
        public void NotForwardAdd()
        {
            Sut.MyEvent += MyEventHandler;
            Assert.Equal(0, Adds.Count);
        }

        [Fact]
        public void ForwardRemove()
        {
            Sut.MyEvent -= MyEventHandler;
            Assert.Equal(1, Removes.Count);
        }
    }
}
