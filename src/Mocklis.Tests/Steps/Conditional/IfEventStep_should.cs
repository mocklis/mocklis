// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfEventStep_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Stored;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class IfEventStep_should
    {
        private int _firstEventHandlerCallCount;
        private int _secondEventHandlerCallCount;

        private void MyFirstEventHandler(object sender, EventArgs e)
        {
            _firstEventHandlerCallCount++;
        }

        private void MySecondEventHandler(object sender, EventArgs e)
        {
            _secondEventHandlerCallCount++;
        }

        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        [Fact]
        public void check_common_condition()
        {
            StoredEventStep<EventHandler> eventStore = null;
            MockMembers.MyEvent.If(e => e.Method.Name == nameof(MyFirstEventHandler), i => i.Stored(out eventStore)).Dummy();

            Sut.MyEvent += MyFirstEventHandler;
            Sut.MyEvent += MySecondEventHandler;
            eventStore.Raise(this, EventArgs.Empty);
            Sut.MyEvent -= MyFirstEventHandler;
            Sut.MyEvent -= MySecondEventHandler;
            eventStore.Raise(this, EventArgs.Empty);

            Assert.Equal(1, _firstEventHandlerCallCount);
            Assert.Equal(0, _secondEventHandlerCallCount);
        }

        [Fact]
        public void check_separate_conditions()
        {
            StoredEventStep<EventHandler> eventStore = null;
            MockMembers.MyEvent.If(
                e => e.Method.Name == nameof(MyFirstEventHandler),
                e => e.Method.Name == nameof(MySecondEventHandler),
                i => i.Stored(out eventStore)).Dummy();

            // This only adds first event handler
            Sut.MyEvent += MyFirstEventHandler;
            Sut.MyEvent += MySecondEventHandler;
            eventStore.Raise(this, EventArgs.Empty);

            // This tries to remove second handler; as it's not there the store remains unchanged.
            Sut.MyEvent -= MyFirstEventHandler;
            Sut.MyEvent -= MySecondEventHandler;
            eventStore.Raise(this, EventArgs.Empty);

            Assert.Equal(2, _firstEventHandlerCallCount);
            Assert.Equal(0, _secondEventHandlerCallCount);
        }
    }
}
