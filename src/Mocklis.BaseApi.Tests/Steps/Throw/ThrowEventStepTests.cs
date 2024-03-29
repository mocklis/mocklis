// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ThrowEventStepTests
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IEvents Sut => MockMembers;
        private readonly EventHandler _eventHandler = (sender, args) => { };

        [Fact]
        public void RequireExceptionFactory()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.Throw(null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void ThrowOnAdd()
        {
            MockMembers.MyEvent.Throw(handler => new SampleException<EventHandler?>(handler));
            var ex = Assert.Throws<SampleException<EventHandler>>(() => Sut.MyEvent += _eventHandler);
            Assert.Equal(_eventHandler, ex.Payload);
        }

        [Fact]
        public void ThrowOnRemove()
        {
            MockMembers.MyEvent.Throw(handler => new SampleException<EventHandler?>(handler));
            var ex = Assert.Throws<SampleException<EventHandler>>(() => Sut.MyEvent -= _eventHandler);
            Assert.Equal(_eventHandler, ex.Payload);
        }

        [Fact]
        public void RequireExceptionFactoryWithInstance()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.InstanceThrow(null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void ThrowOnAddWithInstance()
        {
            MockMembers.MyEvent.InstanceThrow((i, handler) => new SampleException<EventHandler?>(handler, i));
            var ex = Assert.Throws<SampleException<EventHandler>>(() => Sut.MyEvent += _eventHandler);
            Assert.Equal(_eventHandler, ex.Payload);
            Assert.Same(MockMembers, ex.Instance);
        }

        [Fact]
        public void ThrowOnRemoveWithInstance()
        {
            MockMembers.MyEvent.InstanceThrow((i, handler) => new SampleException<EventHandler?>(handler, i));
            var ex = Assert.Throws<SampleException<EventHandler>>(() => Sut.MyEvent -= _eventHandler);
            Assert.Equal(_eventHandler, ex.Payload);
            Assert.Same(MockMembers, ex.Instance);
        }
    }
}
