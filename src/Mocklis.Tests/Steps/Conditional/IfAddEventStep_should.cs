// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfAddEventStep_should.cs">
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
        private void MyEventHandler(object sender, EventArgs e)
        {
        }

        private IEvents Sut { get; }
        private IReadOnlyList<EventHandler> _adds;
        private IReadOnlyList<EventHandler> _removes;

        public IfAddEventStep_should()
        {
            var mockMembers = new MockMembers();

            mockMembers.MyEvent
                .IfAdd(i => i
                    .RecordBeforeAdd(out _adds, a => a)
                    .RecordBeforeRemove(out _removes, a => a)
                    .Join(i.ElseBranch))
                .Dummy();

            Sut = mockMembers;
        }

        [Fact]
        public void forward_Add()
        {
            Sut.MyEvent += MyEventHandler;
            Assert.Equal(1, _adds.Count);
        }

        [Fact]
        public void not_forward_Remove()
        {
            Sut.MyEvent -= MyEventHandler;
            Assert.Equal(0, _removes.Count);
        }
    }
}
