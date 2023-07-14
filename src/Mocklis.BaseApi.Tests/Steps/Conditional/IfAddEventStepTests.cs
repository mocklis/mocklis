// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfAddEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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

    public class IfAddEventStepTests
    {
        private readonly EventHandler _handler = (sender, args) => { };

        private IEvents Sut { get; }
        private IReadOnlyList<EventHandler?> Adds { get; }
        private IReadOnlyList<EventHandler?> Removes { get; }

        public IfAddEventStepTests()
        {
            var mockMembers = new MockMembers();

            IReadOnlyList<EventHandler?>? adds = null;
            IReadOnlyList<EventHandler?>? removes = null;

            mockMembers.MyEvent
                .IfAdd(i => i
                    .RecordBeforeAdd(out adds)
                    .RecordBeforeRemove(out removes)
                    .Join(i.ElseBranch));

            Adds = adds!;
            Removes = removes!;
            Sut = mockMembers;
        }

        [Fact]
        public void ForwardAdd()
        {
            Sut.MyEvent += _handler;
            Assert.Single(Adds);
        }

        [Fact]
        public void NotForwardRemove()
        {
            Sut.MyEvent -= _handler;
            Assert.Empty(Removes);
        }
    }
}
