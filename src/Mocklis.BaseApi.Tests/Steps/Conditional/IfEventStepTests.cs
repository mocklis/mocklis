// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using System.Reflection;
    using Mocklis.Core;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Mocklis.Steps.Stored;
    using Xunit;

    #endregion

    public class IfEventStepTests
    {
        private int _firstEventHandlerCallCount;
        private int _secondEventHandlerCallCount;

        private void MyFirstEventHandler(object? sender, EventArgs e)
        {
            _firstEventHandlerCallCount++;
        }

        private void MySecondEventHandler(object? sender, EventArgs e)
        {
            _secondEventHandlerCallCount++;
        }

        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        [Fact]
        public void RequireBranch()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.If(_ => true, null, null!));
        }

        [Fact]
        public void CheckCommonCondition()
        {
            StoredEventStep<EventHandler>? eventStore = null;
            MockMembers.MyEvent.If(e => e?.GetMethodInfo()?.Name == nameof(MyFirstEventHandler), i => i.Stored(out eventStore));

            Sut.MyEvent += MyFirstEventHandler;
            Sut.MyEvent += MySecondEventHandler;
            eventStore!.Raise(this, EventArgs.Empty);
            Sut.MyEvent -= MyFirstEventHandler;
            Sut.MyEvent -= MySecondEventHandler;
            eventStore!.Raise(this, EventArgs.Empty);

            Assert.Equal(1, _firstEventHandlerCallCount);
            Assert.Equal(0, _secondEventHandlerCallCount);
        }

        [Fact]
        public void CheckSeparateConditions()
        {
            StoredEventStep<EventHandler>? eventStore = null;
            MockMembers.MyEvent.If(
                e => e?.GetMethodInfo()?.Name == nameof(MyFirstEventHandler),
                e => e?.GetMethodInfo()?.Name == nameof(MySecondEventHandler),
                i => i.Stored(out eventStore));

            // This only adds first event handler
            Sut.MyEvent += MyFirstEventHandler;
            Sut.MyEvent += MySecondEventHandler;
            eventStore?.Raise(this, EventArgs.Empty);

            // This tries to remove second handler; as it's not there the store remains unchanged.
            Sut.MyEvent -= MyFirstEventHandler;
            Sut.MyEvent -= MySecondEventHandler;
            eventStore?.Raise(this, EventArgs.Empty);

            Assert.Equal(2, _firstEventHandlerCallCount);
            Assert.Equal(0, _secondEventHandlerCallCount);
        }

        [Fact]
        public void ThrowWhenPassedNullAsNextStep()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MockMembers.MyEvent.If(
                    e => true,
                    e => true,
                    s => ((ICanHaveNextEventStep<EventHandler>)s).SetNextStep((IEventStep<EventHandler>)null!)
                )
            );
        }
    }
}
