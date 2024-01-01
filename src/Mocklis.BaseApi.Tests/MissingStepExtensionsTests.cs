// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingStepExtensionsTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Mocks;
    using Mocklis.Steps.Missing;
    using Xunit;

    #endregion

    public class MissingStepExtensionsTests
    {
        [Fact]
        public void ForEventsSetMissingInstanceAsNextStep()
        {
            // Arrange
            var eventMock = new MockCanHaveNextEventStep<EventHandler>();
            eventMock.SetNextStep<MissingEventStep<EventHandler>>().RecordBeforeCall(out var ledger);

            // Act
            eventMock.Missing();

            // Assert
            var item = Assert.Single(ledger);
            Assert.Same(MissingEventStep<EventHandler>.Instance, item);
        }

        [Fact]
        public void ForIndexersSetMissingInstanceAsNextStep()
        {
            // Arrange
            var indexerMock = new MockCanHaveNextIndexerStep<int, string>();
            indexerMock.SetNextStep<MissingIndexerStep<int, string>>().RecordBeforeCall(out var ledger);

            // Act
            indexerMock.Missing();

            // Assert
            var item = Assert.Single(ledger);
            Assert.Same(MissingIndexerStep<int, string>.Instance, item);
        }

        [Fact]
        public void ForMethodsSetMissingInstanceAsNextStep()
        {
            // Arrange
            var eventMock = new MockCanHaveNextMethodStep<int, string>();
            eventMock.SetNextStep<MissingMethodStep<int, string>>().RecordBeforeCall(out var ledger);

            // Act
            eventMock.Missing();

            // Assert
            var item = Assert.Single(ledger);
            Assert.Same(MissingMethodStep<int, string>.Instance, item);
        }

        [Fact]
        public void ForPropertiesSetMissingInstanceAsNextStep()
        {
            // Arrange
            var indexerMock = new MockCanHaveNextPropertyStep<int>();
            indexerMock.SetNextStep<MissingPropertyStep<int>>().RecordBeforeCall(out var ledger);

            // Act
            indexerMock.Missing();

            // Assert
            var item = Assert.Single(ledger);
            Assert.Same(MissingPropertyStep<int>.Instance, item);
        }
    }
}
