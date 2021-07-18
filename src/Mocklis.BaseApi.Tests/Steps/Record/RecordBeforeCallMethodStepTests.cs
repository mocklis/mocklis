// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeCallMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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

    public class RecordBeforeCallMethodStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IMethods _methods;

        public RecordBeforeCallMethodStepTests()
        {
            _methods = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _mockMembers.FuncWithParameter.RecordBeforeCall(out IReadOnlyList<int> _, null!);
            });

            // Assert
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void RecordBasicSuccessInformationWithoutSelectors()
        {
            // Arrange
            _mockMembers.FuncWithParameter.RecordBeforeCall(out var ledger)
                .ReturnOnce(55)
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            _methods.FuncWithParameter(16);
            Assert.Throws<Exception>(() => _methods.FuncWithParameter(17));

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Equal(16, ledger[0]);
            Assert.Equal(17, ledger[1]);
        }

        [Fact]
        public void RecordParameterAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.FuncWithParameter
                .RecordBeforeCall(out var ledger, GenericRecord<int, int>.One)
                .ReturnOnce(55)
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            var result = _methods.FuncWithParameter(15);
            Assert.Throws<Exception>(() => _methods.FuncWithParameter(25));

            // Assert
            Assert.Equal(55, result);
            Assert.Equal(2, ledger.Count);
            Assert.Equal(15, ledger[0].Data1);
            Assert.Equal(25, ledger[1].Data1);
        }
    }
}
