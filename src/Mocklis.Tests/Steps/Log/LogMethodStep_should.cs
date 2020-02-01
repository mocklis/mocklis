// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogMethodStep_should.cs">
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

    public class LogMethodStep_should
    {
        private readonly MockLogContextProvider _logContextProvider = new MockLogContextProvider();
        private readonly MockLogContext _logContext = new MockLogContext();
        private readonly MockMembers _mockMembers;
        private readonly IMethods _methods;

        public LogMethodStep_should()
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

        [AssertionMethod]
        private void AssertMockInfoIsCorrectForFunc(IMockInfo mockInfo)
        {
            Assert.Same(_methods, mockInfo.MockInstance);
            Assert.Equal(nameof(MockMembers), mockInfo.MocklisClassName);
            Assert.Equal(nameof(IMethods), mockInfo.InterfaceName);
            Assert.Equal(nameof(IMethods.FuncWithParameter), mockInfo.MemberName);
            Assert.Equal(nameof(MockMembers.FuncWithParameter), mockInfo.MemberMockName);
        }

        [AssertionMethod]
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
            Assert.Equal(1, before.Count);
            Assert.Equal(9, before[0].param);
            Assert.Equal(5, result);
            AssertMockInfoIsCorrectForFunc(before[0].mockInfo);
            Assert.Equal(1, after.Count);
            Assert.Equal(5, after[0].result);
            AssertMockInfoIsCorrectForFunc(after[0].mockInfo);
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
            Assert.Equal(1, before.Count);
            AssertMockInfoIsCorrectForAction(before[0]);
            Assert.Equal(1, after.Count);
            AssertMockInfoIsCorrectForAction(after[0]);
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
            Assert.Equal(1, before.Count);
            Assert.Equal(9, before[0].param);
            AssertMockInfoIsCorrectForFunc(before[0].mockInfo);
            Assert.Equal(1, exceptions.Count);
            Assert.Same(ex, exceptions[0].exception);
            AssertMockInfoIsCorrectForFunc(exceptions[0].mockInfo);
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
            Assert.Equal(1, before.Count);
        }
    }
}
