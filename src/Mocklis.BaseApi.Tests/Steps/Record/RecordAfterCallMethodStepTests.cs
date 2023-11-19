// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterCallMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class RecordAfterCallMethodStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IMethods _methods;

        public RecordAfterCallMethodStepTests()
        {
            _methods = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSuccessSelectorIfNoFailureSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                _mockMembers.FuncWithParameter.RecordAfterCall(out IReadOnlyList<int> _, null);
            });

            // Assert
            Assert.Equal("successSelector", ex.ParamName);
        }

        [Fact]
        public void RecordBasicSuccessInformationWithoutSelectors()
        {
            // Arrange
            _mockMembers.FuncWithParameter.RecordAfterCall(out var ledger)
                .ReturnOnce(55)
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            var result = _methods.FuncWithParameter(16);
            Assert.Throws<Exception>(() => _methods.FuncWithParameter(17));

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Equal(16, ledger[0].Parameter);
            Assert.Equal(result, ledger[0].Result);
        }

        [Fact]
        public void RecordReturnValuesAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.FuncWithParameter
                .RecordAfterCall(out var ledger, GenericRecord<int, int>.Two, GenericRecord<int, int>.Ex)
                .ReturnOnce(55)
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            var result = _methods.FuncWithParameter(15);
            var ex = Assert.Throws<Exception>(() => _methods.FuncWithParameter(25));

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.True(ledger[0].IsSuccess);
            Assert.Equal(15, ledger[0].Data1);
            Assert.Equal(result, ledger[0].Data2);
            Assert.False(ledger[1].IsSuccess);
            Assert.Equal(25, ledger[1].Data1);
            Assert.Same(ex, ledger[1].Exception);
        }

        [Fact]
        public void NotRecordExceptionsIfFailureSelectorNotGiven()
        {
            // Arrange
            _mockMembers.FuncWithParameter
                .RecordAfterCall(out var ledger, GenericRecord<int, int>.Two)
                .Times(1, a => a.Throw(_ => new Exception("First exception thrown!")))
                .ReturnOnce(55)
                .Throw(_ => new Exception("Second exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _methods.FuncWithParameter(15));
            var result = _methods.FuncWithParameter(20);
            Assert.Throws<Exception>(() => _methods.FuncWithParameter(25));

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.True(ledger[0].IsSuccess);
            Assert.Equal(20, ledger[0].Data1);
            Assert.Equal(result, ledger[0].Data2);
        }

        [Fact]
        public void OnlyRecordExceptionsIfSuccessSelectorNotGiven()
        {
            // Arrange
            _mockMembers.FuncWithParameter
                .RecordAfterCall(out var ledger, null, GenericRecord<int, int>.Ex)
                .Times(1, a => a.Throw(_ => new Exception("First exception thrown!")))
                .ReturnOnce(55)
                .Throw(_ => new Exception("Second exception thrown!"));

            // Act
            var ex1 = Assert.Throws<Exception>(() => _methods.FuncWithParameter(15));
            _methods.FuncWithParameter(20);
            var ex2 = Assert.Throws<Exception>(() => _methods.FuncWithParameter(25));

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.False(ledger[0].IsSuccess);
            Assert.Equal(15, ledger[0].Data1);
            Assert.Same(ex1, ledger[0].Exception);
            Assert.False(ledger[1].IsSuccess);
            Assert.Equal(25, ledger[1].Data1);
            Assert.Same(ex2, ledger[1].Exception);
        }
    }
}
