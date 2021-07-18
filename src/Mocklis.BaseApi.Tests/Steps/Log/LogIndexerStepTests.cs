// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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

    public class LogIndexerStepTests
    {
        private readonly MockLogContextProvider _logContextProvider = new MockLogContextProvider();
        private readonly MockLogContext _logContext = new MockLogContext();
        private readonly MockMembers _mockMembers;
        private readonly IIndexers _indexers;

        public LogIndexerStepTests()
        {
            _indexers = _mockMembers = new MockMembers();
            _logContextProvider.LogContext.Return(_logContext);
        }

        [Fact]
        public void RequireLogContext()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new LogIndexerStep<int, string>(null!);
            });

            Assert.Equal("logContext", exception.ParamName);
        }

        private void AssertMockInfoIsCorrect(IMockInfo mockInfo)
        {
            Assert.Same(_indexers, mockInfo.MockInstance);
            Assert.Equal(nameof(MockMembers), mockInfo.MocklisClassName);
            Assert.Equal(nameof(IIndexers), mockInfo.InterfaceName);
            Assert.Equal("this[]", mockInfo.MemberName);
            Assert.Equal("Item", mockInfo.MemberMockName);
        }

        [Fact]
        public void LogBeforeAndAfterOnSet()
        {
            // Arrange
            _mockMembers.Item.Log(_logContext);
            _logContext.LogBeforeIndexerSet<int, string>().RecordBeforeCall(out var before);
            _logContext.LogAfterIndexerSet.RecordBeforeCall(out var after);

            // Act
            _indexers[5] = "Test";

            // Assert
            Assert.Equal(1, before.Count);
            Assert.Equal(5, before[0].key);
            Assert.Equal("Test", before[0].value);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, after.Count);
            AssertMockInfoIsCorrect(after[0]);
        }

        [Fact]
        public void LogBeforeAndExceptionOnSet()
        {
            // Arrange
            _mockMembers.Item.Log(_logContext).Throw(key => new Exception("Exception thrown!"));
            _logContext.LogBeforeIndexerSet<int, string>().RecordBeforeCall(out var before);
            _logContext.LogIndexerSetException.RecordBeforeCall(out var exceptions);

            // Act
            var ex = Assert.Throws<Exception>(() => _indexers[5] = "Test");

            // Assert
            Assert.Equal(1, before.Count);
            Assert.Equal(5, before[0].key);
            Assert.Equal("Test", before[0].value);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, exceptions.Count);
            Assert.Same(ex, exceptions[0].exception);
            AssertMockInfoIsCorrect(exceptions[0].mockInfo);
        }

        [Fact]
        public void LogBeforeAndAfterOnGet()
        {
            // Arrange
            _mockMembers.Item.Log(_logContext).Return("Test");
            _logContext.LogBeforeIndexerGet<int>().RecordBeforeCall(out var before);
            _logContext.LogAfterIndexerGet<string>().RecordBeforeCall(out var after);

            // Act
            var _ = _indexers[5];

            // Assert
            Assert.Equal(1, before.Count);
            Assert.Equal(5, before[0].key);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, after.Count);
            Assert.Equal("Test", after[0].value);
            AssertMockInfoIsCorrect(after[0].mockInfo);
        }

        [Fact]
        public void LogBeforeAndExceptionOnGet()
        {
            // Arrange
            _mockMembers.Item.Log(_logContext).Throw(key => new Exception("Exception thrown!"));
            _logContext.LogBeforeIndexerGet<int>().RecordBeforeCall(out var before);
            _logContext.LogIndexerGetException.RecordBeforeCall(out var exceptions);

            // Act
            var ex = Assert.Throws<Exception>(() => _indexers[5]);

            // Assert
            Assert.Equal(1, before.Count);
            Assert.Equal(5, before[0].key);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, exceptions.Count);
            Assert.Same(ex, exceptions[0].exception);
            AssertMockInfoIsCorrect(exceptions[0].mockInfo);
        }

        [Fact]
        public void UseLogContextProvider()
        {
            // Arrange
            _mockMembers.Item.Log(_logContextProvider);
            _logContext.LogBeforeIndexerSet<int, string>().RecordBeforeCall(out var before);

            // Act
            _indexers[5] = "Test";

            // Assert
            Assert.Equal(1, before.Count);
        }
    }
}
