// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeSetIndexerStepTests.cs">
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

    public class InstanceRecordBeforeSetIndexerStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IIndexers _indexers;

        public InstanceRecordBeforeSetIndexerStepTests()
        {
            _indexers = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _mockMembers.Item.InstanceRecordBeforeSet(out IReadOnlyList<int> _, null!);
            });

            // Assert
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void RecordParameterAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.Item
                .InstanceRecordBeforeSet(out var ledger, GenericRecord<int, string>.Two)
                .Times(1, a => a.Dummy())
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            _indexers[15] = "test";
            Assert.Throws<Exception>(() => _indexers[25] = "test");

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Equal(15, ledger[0].Data1);
            Assert.Equal("test", ledger[0].Data2);
            Assert.Same(_mockMembers, ledger[0].Instance);
            Assert.Equal(25, ledger[1].Data1);
            Assert.Equal("test", ledger[1].Data2);
            Assert.Same(_mockMembers, ledger[1].Instance);
        }
    }
}
