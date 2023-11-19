// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class LogEventStepTests
    {
        private readonly MockLogContextProvider _logContextProvider = new MockLogContextProvider();
        private readonly MockLogContext _logContext = new MockLogContext();
        private readonly MockMembers _mockMembers;
        private readonly IEvents _events;
        private readonly EventHandler _sampleEventHandler = (sender, args) => { };

        public LogEventStepTests()
        {
            _events = _mockMembers = new MockMembers();
            _logContextProvider.LogContext.Return(_logContext);
        }

        [Fact]
        public void RequireLogContext()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new LogEventStep<EventHandler>(null!);
            });

            Assert.Equal("logContext", exception.ParamName);
        }

        private void AssertMockInfoIsCorrect(IMockInfo mockInfo)
        {
            Assert.Same(_events, mockInfo.MockInstance);
            Assert.Equal(nameof(MockMembers), mockInfo.MocklisClassName);
            Assert.Equal(nameof(IEvents), mockInfo.InterfaceName);
            Assert.Equal(nameof(IEvents.MyEvent), mockInfo.MemberName);
            Assert.Equal(nameof(MockMembers.MyEvent), mockInfo.MemberMockName);
        }

        [Fact]
        public void LogBeforeAndAfterOnAdd()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logContext);
            _logContext.LogBeforeEventAdd<EventHandler>().RecordBeforeCall(out var before);
            _logContext.LogAfterEventAdd.RecordBeforeCall(out var after);

            // Act
            _events.MyEvent += _sampleEventHandler;

            // Assert
            Assert.Equal(1, before.Count);
            Assert.Same(_sampleEventHandler, before[0].value);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, after.Count);
            AssertMockInfoIsCorrect(after[0]);
        }

        [Fact]
        public void LogBeforeAndExceptionOnAdd()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logContext).Throw(handler => new Exception("Exception thrown!"));
            _logContext.LogBeforeEventAdd<EventHandler>().RecordBeforeCall(out var before);
            _logContext.LogEventAddException.RecordBeforeCall(out var exceptions);

            // Act
            var ex = Assert.Throws<Exception>(() => _events.MyEvent += _sampleEventHandler);

            // Assert
            Assert.Equal(1, before.Count);
            Assert.Same(_sampleEventHandler, before[0].value);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, exceptions.Count);
            Assert.Same(ex, exceptions[0].exception);
            AssertMockInfoIsCorrect(exceptions[0].mockInfo);
        }

        [Fact]
        public void LogBeforeAndAfterOnRemove()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logContext);
            _logContext.LogBeforeEventRemove<EventHandler>().RecordBeforeCall(out var before);
            _logContext.LogAfterEventRemove.RecordBeforeCall(out var after);

            // Act
            _events.MyEvent -= _sampleEventHandler;

            // Assert
            Assert.Equal(1, before.Count);
            Assert.Same(_sampleEventHandler, before[0].value);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, after.Count);
            AssertMockInfoIsCorrect(after[0]);
        }

        [Fact]
        public void LogBeforeAndExceptionOnRemove()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logContext).Throw(handler => new Exception("Exception thrown!"));
            _logContext.LogBeforeEventRemove<EventHandler>().RecordBeforeCall(out var before);
            _logContext.LogEventRemoveException.RecordBeforeCall(out var exceptions);

            // Act
            var ex = Assert.Throws<Exception>(() => _events.MyEvent -= _sampleEventHandler);

            // Assert
            Assert.Equal(1, before.Count);
            Assert.Same(_sampleEventHandler, before[0].value);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, exceptions.Count);
            Assert.Same(ex, exceptions[0].exception);
            AssertMockInfoIsCorrect(exceptions[0].mockInfo);
        }

        [Fact]
        public void UseLogContextProvider()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logContextProvider);
            _logContext.LogBeforeEventAdd<EventHandler>().RecordBeforeCall(out var before);

            // Act
            _events.MyEvent += _sampleEventHandler;

            // Assert
            Assert.Equal(1, before.Count);
        }
    }
}
