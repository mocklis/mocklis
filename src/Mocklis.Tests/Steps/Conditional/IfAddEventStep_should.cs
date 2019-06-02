// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfAddEventStep_should.cs">
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

    public class IfAddEventStep_should
    {
        private readonly EventHandler _handler = (sender, args) => { };

        private IEvents Sut { get; }
        private IReadOnlyList<EventHandler> _adds;
        private IReadOnlyList<EventHandler> _removes;

        public IfAddEventStep_should()
        {
            var mockMembers = new MockMembers();

            mockMembers.MyEvent
                .IfAdd(i => i
                    .RecordBeforeAdd(out _adds)
                    .RecordBeforeRemove(out _removes)
                    .Join(i.ElseBranch));

            Sut = mockMembers;
        }

        [Fact]
        public void forward_Add()
        {
            Sut.MyEvent += _handler;
            Assert.Equal(1, _adds.Count);
        }

        [Fact]
        public void not_forward_Remove()
        {
            Sut.MyEvent -= _handler;
            Assert.Equal(0, _removes.Count);
        }
    }
}
