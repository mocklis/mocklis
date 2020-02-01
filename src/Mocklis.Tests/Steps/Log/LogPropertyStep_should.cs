// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Log
{
    #region Using Directives

    using System;
    using JetBrains.Annotations;
    using Mocklis.Core;
    using Mocklis.Steps.Log;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class LogPropertyStep_should
    {
        private readonly MockLogContextProvider _logContextProvider = new MockLogContextProvider();
        private readonly MockLogContext _logContext = new MockLogContext();
        private readonly MockMembers _mockMembers;
        private readonly IProperties _properties;

        public LogPropertyStep_should()
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

        [AssertionMethod]
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
            Assert.Equal(1, before.Count);
            Assert.Equal("Test", before[0].value);
            AssertMockInfoIsCorrect(before[0].mockInfo);
            Assert.Equal(1, after.Count);
            AssertMockInfoIsCorrect(after[0]);
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
            Assert.Equal(1, before.Count);
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
            _mockMembers.StringProperty.Log(_logContext).Return("Test");
            _logContext.LogBeforePropertyGet.RecordBeforeCall(out var before);
            _logContext.LogAfterPropertyGet<string>().RecordBeforeCall(out var after);

            // Act
            var _ = _properties.StringProperty;

            // Assert
            Assert.Equal(1, before.Count);
            AssertMockInfoIsCorrect(before[0]);
            Assert.Equal(1, after.Count);
            Assert.Equal("Test", after[0].value);
            AssertMockInfoIsCorrect(after[0].mockInfo);
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
            Assert.Equal(1, before.Count);
            AssertMockInfoIsCorrect(before[0]);
            Assert.Equal(1, exceptions.Count);
            Assert.Same(ex, exceptions[0].exception);
            AssertMockInfoIsCorrect(exceptions[0].mockInfo);
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
            Assert.Equal(1, before.Count);
        }
    }
}
