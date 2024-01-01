// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeSetIndexerStepTests.cs">
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

    public class RecordBeforeSetIndexerStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IIndexers _indexers;

        public RecordBeforeSetIndexerStepTests()
        {
            _indexers = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _mockMembers.Item.RecordBeforeSet(out IReadOnlyList<int> _, null!);
            });

            // Assert
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void RecordBasicSuccessInformationWithoutSelectors()
        {
            // Arrange
            _mockMembers.Item.RecordBeforeSet(out var ledger)
                .Times(1, a => a.Dummy())
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            _indexers[16] = "test";
            Assert.Throws<Exception>(() => _indexers[17] = "test");

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Equal(16, ledger[0].Key);
            Assert.Equal("test", ledger[0].Value);
            Assert.Equal(17, ledger[1].Key);
            Assert.Equal("test", ledger[1].Value);
        }

        [Fact]
        public void RecordParameterAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.Item
                .RecordBeforeSet(out var ledger, GenericRecord<int, string>.Two)
                .Times(1, a => a.Dummy())
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            _indexers[15] = "test";
            Assert.Throws<Exception>(() => _indexers[25] = "test");

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Equal(15, ledger[0].Data1);
            Assert.Equal("test", ledger[0].Data2);
            Assert.Equal(25, ledger[1].Data1);
            Assert.Equal("test", ledger[1].Data2);
        }
    }
}
