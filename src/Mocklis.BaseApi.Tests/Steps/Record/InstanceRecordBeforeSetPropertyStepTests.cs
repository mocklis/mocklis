// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeSetPropertyStepTests.cs">
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

    public class InstanceRecordBeforeSetPropertyStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IProperties _properties;

        public InstanceRecordBeforeSetPropertyStepTests()
        {
            _properties = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _mockMembers.StringProperty.InstanceRecordBeforeSet(out IReadOnlyList<int> _, null!);
            });

            // Assert
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void RecordParameterAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.StringProperty
                .InstanceRecordBeforeSet(out var ledger, GenericRecord<string>.One)
                .Times(1, a => a.Dummy())
                .Throw(() => new Exception("Exception thrown!"));

            // Act
            _properties.StringProperty = "test";
            Assert.Throws<Exception>(() => _properties.StringProperty = "test");

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Equal("test", ledger[0].Data);
            Assert.Same(_mockMembers, ledger[0].Instance);
            Assert.Equal("test", ledger[1].Data);
            Assert.Same(_mockMembers, ledger[1].Instance);
        }
    }
}
