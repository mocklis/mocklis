// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfRemoveEventStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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

    public class IfRemoveEventStep_should
    {
        private void MyEventHandler(object? sender, EventArgs e)
        {
        }

        private IEvents Sut { get; }
        private IReadOnlyList<EventHandler?> Adds { get; }
        private IReadOnlyList<EventHandler?> Removes { get; }

        public IfRemoveEventStep_should()
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
        public void not_forward_Add()
        {
            Sut.MyEvent += MyEventHandler;
            Assert.Equal(0, Adds.Count);
        }

        [Fact]
        public void forward_Remove()
        {
            Sut.MyEvent -= MyEventHandler;
            Assert.Equal(1, Removes.Count);
        }
    }
}
