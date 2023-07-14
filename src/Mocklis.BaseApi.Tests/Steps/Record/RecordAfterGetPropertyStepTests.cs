// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterGetPropertyStepTests.cs">
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

    public class RecordAfterGetPropertyStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IProperties _properties;

        public RecordAfterGetPropertyStepTests()
        {
            _properties = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSuccessSelectorIfNoFailureSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                _mockMembers.IntProperty.RecordAfterGet(out IReadOnlyList<int> _, null);
            });

            // Assert
            Assert.Equal("successSelector", ex.ParamName);
        }

        [Fact]
        public void RecordBasicSuccessInformationWithoutSelectors()
        {
            // Arrange
            _mockMembers.IntProperty.RecordAfterGet(out var ledger)
                .ReturnOnce(10)
                .Throw(() => new Exception("Exception thrown!"));

            // Act
            var result = _properties.IntProperty;
            Assert.Throws<Exception>(() => _properties.IntProperty);

            // Assert
            var item = Assert.Single(ledger);
            Assert.Equal(result, item);
        }

        [Fact]
        public void RecordReturnValuesAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.IntProperty
                .RecordAfterGet(out var ledger, GenericRecord<int>.One, GenericRecord<int>.Ex)
                .ReturnOnce(10)
                .Throw(() => new Exception("Exception thrown!"));

            // Act
            var result = _properties.IntProperty;
            var ex = Assert.Throws<Exception>(() => _properties.IntProperty);

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.True(ledger[0].IsSuccess);
            Assert.Equal(result, ledger[0].Data);
            Assert.False(ledger[1].IsSuccess);
            Assert.Same(ex, ledger[1].Exception);
        }

        [Fact]
        public void NotRecordExceptionsIfFailureSelectorNotGiven()
        {
            // Arrange
            _mockMembers.IntProperty
                .RecordAfterGet(out var ledger, GenericRecord<int>.One)
                .Times(1, a => a.Throw(() => new Exception("First exception thrown!")))
                .ReturnOnce(10)
                .Throw(() => new Exception("Second exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _properties.IntProperty);
            var result = _properties.IntProperty;
            Assert.Throws<Exception>(() => _properties.IntProperty);

            // Assert
            var item = Assert.Single(ledger);
            Assert.True(item.IsSuccess);
            Assert.Equal(result, item.Data);
        }

        [Fact]
        public void OnlyRecordExceptionsIfSuccessSelectorNotGiven()
        {
            // Arrange
            _mockMembers.IntProperty
                .RecordAfterGet(out var ledger, null, GenericRecord<int>.Ex)
                .Times(1, a => a.Throw(() => new Exception("First exception thrown!")))
                .ReturnOnce(10)
                .Throw(() => new Exception("Second exception thrown!"));

            // Act
            var ex1 = Assert.Throws<Exception>(() => _properties.IntProperty);
            var result = _properties.IntProperty;
            var ex2 = Assert.Throws<Exception>(() => _properties.IntProperty);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal(2, ledger.Count);
            Assert.False(ledger[0].IsSuccess);
            Assert.Same(ex1, ledger[0].Exception);
            Assert.False(ledger[1].IsSuccess);
            Assert.Same(ex2, ledger[1].Exception);
        }
    }
}
