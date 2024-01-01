// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterGetIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
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

    public class RecordAfterGetIndexerStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IIndexers _indexers;

        public RecordAfterGetIndexerStepTests()
        {
            _indexers = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSuccessSelectorIfNoFailureSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                _mockMembers.Item.RecordAfterGet(out IReadOnlyList<int> _, null);
            });

            // Assert
            Assert.Equal("successSelector", ex.ParamName);
        }

        [Fact]
        public void RecordBasicSuccessInformationWithoutSelectors()
        {
            // Arrange
            _mockMembers.Item.RecordAfterGet(out var ledger)
                .ReturnOnce("aha")
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            var result = _indexers[16];
            Assert.Throws<Exception>(() => _indexers[17]);

            // Assert
            var item = Assert.Single(ledger);
            Assert.Equal(16, item.Key);
            Assert.Equal(result, item.Value);
        }

        [Fact]
        public void RecordReturnValuesAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.Item
                .RecordAfterGet(out var ledger, GenericRecord<int, string>.Two, GenericRecord<int, string>.Ex)
                .ReturnOnce("aha")
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            var result = _indexers[15];
            var ex = Assert.Throws<Exception>(() => _indexers[25]);

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
            _mockMembers.Item
                .RecordAfterGet(out var ledger, GenericRecord<int, string>.Two)
                .Times(1, a => a.Throw(_ => new Exception("First exception thrown!")))
                .ReturnOnce("aha")
                .Throw(_ => new Exception("Second exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _indexers[15]);
            var result = _indexers[20];
            Assert.Throws<Exception>(() => _indexers[25]);

            // Assert
            var item = Assert.Single(ledger);
            Assert.True(item.IsSuccess);
            Assert.Equal(20, item.Data1);
            Assert.Equal(result, item.Data2);
        }

        [Fact]
        public void OnlyRecordExceptionsIfSuccessSelectorNotGiven()
        {
            // Arrange
            _mockMembers.Item
                .RecordAfterGet(out var ledger, null, GenericRecord<int, string>.Ex)
                .Times(1, a => a.Throw(_ => new Exception("First exception thrown!")))
                .ReturnOnce("aha")
                .Throw(_ => new Exception("Second exception thrown!"));

            // Act
            var ex1 = Assert.Throws<Exception>(() => _indexers[15]);
            var result = _indexers[20];
            var ex2 = Assert.Throws<Exception>(() => _indexers[25]);

            // Assert
            Assert.Equal("aha", result);
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
