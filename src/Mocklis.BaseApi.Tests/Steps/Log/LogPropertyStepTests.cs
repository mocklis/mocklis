// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPropertyStepTests.cs">
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

    public class LogPropertyStepTests
    {
        private readonly MockLogContextProvider _logContextProvider = new MockLogContextProvider();
        private readonly MockLogContext _logContext = new MockLogContext();
        private readonly MockMembers _mockMembers;
        private readonly IProperties _properties;

        public LogPropertyStepTests()
        {
            _properties = _mockMembers = new MockMembers();
            _logContextProvider.LogContext.Return(_logContext);
        }

        [Fact]
        public void RequireLogContext()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new LogPropertyStep<string>(null!);
            });

            Assert.Equal("logContext", exception.ParamName);
        }

        private void AssertMockInfoIsCorrect(IMockInfo mockInfo)
        {
            Assert.Same(_properties, mockInfo.MockInstance);
            Assert.Equal(nameof(MockMembers), mockInfo.MocklisClassName);
            Assert.Equal(nameof(IProperties), mockInfo.InterfaceName);
            Assert.Equal(nameof(IProperties.StringProperty), mockInfo.MemberName);
            Assert.Equal(nameof(MockMembers.StringProperty), mockInfo.MemberMockName);
        }

        [Fact]
        public void LogBeforeAndAfterOnSet()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logContext);
            _logContext.LogBeforePropertySet<string>().RecordBeforeCall(out var before);
            _logContext.LogAfterPropertySet.RecordBeforeCall(out var after);

            // Act
            _properties.StringProperty = "Test";

            // Assert
            var beforeItem = Assert.Single(before);
            Assert.Equal("Test", beforeItem.value);
            AssertMockInfoIsCorrect(beforeItem.mockInfo);
            var afterItem = Assert.Single(after);
            AssertMockInfoIsCorrect(afterItem);
        }

        [Fact]
        public void LogBeforeAndExceptionOnSet()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logContext).Throw(() => new Exception("Exception thrown!"));
            _logContext.LogBeforePropertySet<string>().RecordBeforeCall(out var before);
            _logContext.LogPropertySetException.RecordBeforeCall(out var exceptions);

            // Act
            var ex = Assert.Throws<Exception>(() => _properties.StringProperty = "Test");

            // Assert
            var beforeItem = Assert.Single(before);
            Assert.Equal("Test", beforeItem.value);
            AssertMockInfoIsCorrect(beforeItem.mockInfo);
            var exceptionItem = Assert.Single(exceptions);
            Assert.Same(ex, exceptionItem.exception);
            AssertMockInfoIsCorrect(exceptionItem.mockInfo);
        }

        [Fact]
        public void LogBeforeAndAfterOnGet()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logContext).Return("Test");
            _logContext.LogBeforePropertyGet.RecordBeforeCall(out var before);
            _logContext.LogAfterPropertyGet<string>().RecordBeforeCall(out var after);

            // Act
            var _ = _properties.StringProperty;

            // Assert
            var beforeItem = Assert.Single(before);
            AssertMockInfoIsCorrect(beforeItem);
            var afterItem = Assert.Single(after);
            Assert.Equal("Test", afterItem.value);
            AssertMockInfoIsCorrect(afterItem.mockInfo);
        }

        [Fact]
        public void LogBeforeAndExceptionOnGet()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logContext).Throw(() => new Exception("Exception thrown!"));
            _logContext.LogBeforePropertyGet.RecordBeforeCall(out var before);
            _logContext.LogPropertyGetException.RecordBeforeCall(out var exceptions);

            // Act
            var ex = Assert.Throws<Exception>(() => _properties.StringProperty);

            // Assert
            var beforeItem = Assert.Single(before);
            AssertMockInfoIsCorrect(beforeItem);
            var exceptionItem = Assert.Single(exceptions);
            Assert.Same(ex, exceptionItem.exception);
            AssertMockInfoIsCorrect(exceptionItem.mockInfo);
        }

        [Fact]
        public void UseLogContextProvider()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logContextProvider);
            _logContext.LogBeforePropertySet<string>().RecordBeforeCall(out var before);

            // Act
            _properties.StringProperty = "Test";

            // Assert
            Assert.Single(before);
        }
    }
}
