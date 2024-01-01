// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
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

    public class LogMethodStepTests
    {
        private readonly MockLogContextProvider _logContextProvider = new MockLogContextProvider();
        private readonly MockLogContext _logContext = new MockLogContext();
        private readonly MockMembers _mockMembers;
        private readonly IMethods _methods;

        public LogMethodStepTests()
        {
            _methods = _mockMembers = new MockMembers();
            _logContextProvider.LogContext.Return(_logContext);
        }

        [Fact]
        public void RequireLogContext()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new LogMethodStep<int, string>(null!);
            });

            Assert.Equal("logContext", exception.ParamName);
        }

        private void AssertMockInfoIsCorrectForFunc(IMockInfo mockInfo)
        {
            Assert.Same(_methods, mockInfo.MockInstance);
            Assert.Equal(nameof(MockMembers), mockInfo.MocklisClassName);
            Assert.Equal(nameof(IMethods), mockInfo.InterfaceName);
            Assert.Equal(nameof(IMethods.FuncWithParameter), mockInfo.MemberName);
            Assert.Equal(nameof(MockMembers.FuncWithParameter), mockInfo.MemberMockName);
        }

        private void AssertMockInfoIsCorrectForAction(IMockInfo mockInfo)
        {
            Assert.Same(_methods, mockInfo.MockInstance);
            Assert.Equal(nameof(MockMembers), mockInfo.MocklisClassName);
            Assert.Equal(nameof(IMethods), mockInfo.InterfaceName);
            Assert.Equal(nameof(IMethods.SimpleAction), mockInfo.MemberName);
            Assert.Equal(nameof(MockMembers.SimpleAction), mockInfo.MemberMockName);
        }

        [Fact]
        public void LogBeforeAndAfterOnCallWithParameterAndResult()
        {
            // Arrange
            _mockMembers.FuncWithParameter.Log(_logContext).Return(5);
            _logContext.LogBeforeMethodCallWithParameters<int>().RecordBeforeCall(out var before);
            _logContext.LogAfterMethodCallWithResult<int>().RecordBeforeCall(out var after);

            // Act
            var result = _methods.FuncWithParameter(9);

            // Assert
            var beforeItem = Assert.Single(before);
            Assert.Equal(9, beforeItem.param);
            Assert.Equal(5, result);
            AssertMockInfoIsCorrectForFunc(beforeItem.mockInfo);
            var afterItem = Assert.Single(after);
            Assert.Equal(5, afterItem.result);
            AssertMockInfoIsCorrectForFunc(afterItem.mockInfo);
        }

        [Fact]
        public void LogBeforeAndAfterOnCallWithoutParameterOrResult()
        {
            // Arrange
            _mockMembers.SimpleAction.Log(_logContext);
            _logContext.LogBeforeMethodCallWithoutParameters.RecordBeforeCall(out var before);
            _logContext.LogAfterMethodCallWithoutResult.RecordBeforeCall(out var after);

            // Act
            _methods.SimpleAction();

            // Assert
            var beforeItem = Assert.Single(before);
            AssertMockInfoIsCorrectForAction(beforeItem);
            var afterItem = Assert.Single(after);
            AssertMockInfoIsCorrectForAction(afterItem);
        }

        [Fact]
        public void LogBeforeAndExceptionOnCall()
        {
            // Arrange
            _mockMembers.FuncWithParameter.Log(_logContext).Throw(p => new Exception("Exception thrown!"));
            _logContext.LogBeforeMethodCallWithParameters<int>().RecordBeforeCall(out var before);
            _logContext.LogMethodCallException.RecordBeforeCall(out var exceptions);

            // Act
            var ex = Assert.Throws<Exception>(() => _methods.FuncWithParameter(9));

            // Assert
            var beforeItem = Assert.Single(before);
            Assert.Equal(9, beforeItem.param);
            AssertMockInfoIsCorrectForFunc(beforeItem.mockInfo);
            var exceptionItem = Assert.Single(exceptions);
            Assert.Same(ex, exceptionItem.exception);
            AssertMockInfoIsCorrectForFunc(exceptionItem.mockInfo);
        }

        [Fact]
        public void UseLogContextProvider()
        {
            // Arrange
            _mockMembers.FuncWithParameter.Log(_logContextProvider);
            _logContext.LogBeforeMethodCallWithParameters<int>().RecordBeforeCall(out var before);

            // Act
            _methods.FuncWithParameter(9);

            // Assert
            Assert.Single(before);
        }
    }
}
