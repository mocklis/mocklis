// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfAddEventStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class IfAddEventStep_should
    {
        private readonly EventHandler _handler = (sender, args) => { };

        private IEvents Sut { get; }
        private IReadOnlyList<EventHandler?> Adds { get; }
        private IReadOnlyList<EventHandler?> Removes { get; }

        public IfAddEventStep_should()
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
        public void forward_Add()
        {
            Sut.MyEvent += _handler;
            Assert.Equal(1, Adds.Count);
        }

        [Fact]
        public void not_forward_Remove()
        {
            Sut.MyEvent -= _handler;
            Assert.Equal(0, Removes.Count);
        }
    }
}
