// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeSetPropertyStepTests.cs">
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

    public class RecordBeforeSetPropertyStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IProperties _properties;

        public RecordBeforeSetPropertyStepTests()
        {
            _properties = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _mockMembers.StringProperty.RecordBeforeSet(out IReadOnlyList<int> _, null!);
            });

            // Assert
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void RecordBasicSuccessInformationWithoutSelectors()
        {
            // Arrange
            _mockMembers.StringProperty.RecordBeforeSet(out var ledger)
                .Times(1, a => a.Dummy())
                .Throw(() => new Exception("Exception thrown!"));

            // Act
            _properties.StringProperty = "test";
            Assert.Throws<Exception>(() => _properties.StringProperty = "test");

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Equal("test", ledger[0]);
            Assert.Equal("test", ledger[1]);
        }

        [Fact]
        public void RecordParameterAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.StringProperty
                .RecordBeforeSet(out var ledger, GenericRecord<string>.One)
                .Times(1, a => a.Dummy())
                .Throw(() => new Exception("Exception thrown!"));

            // Act
            _properties.StringProperty = "test";
            Assert.Throws<Exception>(() => _properties.StringProperty = "test");

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Equal("test", ledger[0].Data);
            Assert.Equal("test", ledger[1].Data);
        }
    }
}
