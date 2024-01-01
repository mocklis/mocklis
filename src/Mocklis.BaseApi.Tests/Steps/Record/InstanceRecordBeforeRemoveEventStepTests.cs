// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeRemoveEventStepTests.cs">
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

    public class InstanceRecordBeforeRemoveEventStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IEvents _methods;
        private readonly EventHandler _handler = (sender, args) => { };

        public InstanceRecordBeforeRemoveEventStepTests()
        {
            _methods = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _mockMembers.MyEvent.InstanceRecordBeforeRemove(out IReadOnlyList<int> _, null!);
            });

            // Assert
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void RecordParameterAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.MyEvent
                .InstanceRecordBeforeRemove(out var ledger, GenericRecord<EventHandler?>.One)
                .Times(1, a => a.Dummy())
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            _methods.MyEvent -= _handler;
            Assert.Throws<Exception>(() => _methods.MyEvent -= _handler);

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Same(_handler, ledger[0].Data);
            Assert.Same(_mockMembers, ledger[0].Instance);
            Assert.Same(_handler, ledger[1].Data);
            Assert.Same(_mockMembers, ledger[1].Instance);
        }
    }
}
