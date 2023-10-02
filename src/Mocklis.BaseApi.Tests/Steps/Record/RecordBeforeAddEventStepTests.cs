// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeAddEventStepTests.cs">
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

    public class RecordBeforeAddEventStepTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IEvents _methods;
        private readonly EventHandler _handler = (sender, args) => { };

        public RecordBeforeAddEventStepTests()
        {
            _methods = _mockMembers = new MockMembers();
        }

        [Fact]
        public void RequireSelector()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _mockMembers.MyEvent.RecordBeforeAdd(out IReadOnlyList<int> _, null!);
            });

            // Assert
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void RecordBasicSuccessInformationWithoutSelectors()
        {
            // Arrange
            _mockMembers.MyEvent.RecordBeforeAdd(out var ledger)
                .Times(1, a => a.Dummy())
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            _methods.MyEvent += _handler;
            Assert.Throws<Exception>(() => _methods.MyEvent += _handler);

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Same(_handler, ledger[0]);
            Assert.Same(_handler, ledger[1]);
        }

        [Fact]
        public void RecordParameterAndExceptionsUsingSelectors()
        {
            // Arrange
            _mockMembers.MyEvent
                .RecordBeforeAdd(out var ledger, GenericRecord<EventHandler?>.One)
                .Times(1, a => a.Dummy())
                .Throw(_ => new Exception("Exception thrown!"));

            // Act
            _methods.MyEvent += _handler;
            Assert.Throws<Exception>(() => _methods.MyEvent += _handler);

            // Assert
            Assert.Equal(2, ledger.Count);
            Assert.Same(_handler, ledger[0].Data);
            Assert.Same(_handler, ledger[1].Data);
        }
    }
}
