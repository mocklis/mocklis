// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeCallMethodStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Record
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Tests.Helpers;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class InstanceRecordBeforeCallMethodStep_should
    {
        private readonly MockMembers _mockMembers;
        private readonly IMethods _methods;

        public InstanceRecordBeforeCallMethodStep_should()
        {
            _methods = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _mockMembers.FuncWithParameter.InstanceRecordBeforeCall(out IReadOnlyList<int> _, null!);
            });

            // Assert
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void RecordParameterAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.FuncWithParameter
                .InstanceRecordBeforeCall(out var ledger, GenericRecord<int, int>.One)
                .ReturnOnce(55)
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            var result = _methods.FuncWithParameter(15);
            Assert.Throws<Exception>(() => _methods.FuncWithParameter(25));

            // Assert
            Assert.Equal(55, result);
            Assert.Equal(2, ledger.Count);
            Assert.Equal(15, ledger[0].Data1);
            Assert.Same(_mockMembers, ledger[0].Instance);
            Assert.Equal(25, ledger[1].Data1);
            Assert.Same(_mockMembers, ledger[1].Instance);
        }
    }
}
