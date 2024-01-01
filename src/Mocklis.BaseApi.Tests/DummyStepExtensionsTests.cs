// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyStepExtensionsTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Mocks;
    using Mocklis.Steps.Dummy;
    using Xunit;

    #endregion

    public class DummyStepExtensionsTests
    {
        [Fact]
        public void ForEventsSetDummyInstanceAsNextStep()
        {
            // Arrange
            var eventMock = new MockCanHaveNextEventStep<EventHandler>();
            eventMock.SetNextStep<DummyEventStep<EventHandler>>().RecordBeforeCall(out var ledger);

            // Act
            eventMock.Dummy();

            // Assert
            var item = Assert.Single(ledger);
            Assert.Same(DummyEventStep<EventHandler>.Instance, item);
        }

        [Fact]
        public void ForIndexersSetDummyInstanceAsNextStep()
        {
            // Arrange
            var indexerMock = new MockCanHaveNextIndexerStep<int, string>();
            indexerMock.SetNextStep<DummyIndexerStep<int, string>>().RecordBeforeCall(out var ledger);

            // Act
            indexerMock.Dummy();

            // Assert
            var item = Assert.Single(ledger);
            Assert.Same(DummyIndexerStep<int, string>.Instance, item);
        }

        [Fact]
        public void ForMethodsSetDummyInstanceAsNextStep()
        {
            // Arrange
            var eventMock = new MockCanHaveNextMethodStep<int, string>();
            eventMock.SetNextStep<DummyMethodStep<int, string>>().RecordBeforeCall(out var ledger);

            // Act
            eventMock.Dummy();

            // Assert
            var item = Assert.Single(ledger);
            Assert.Same(DummyMethodStep<int, string>.Instance, item);
        }

        [Fact]
        public void ForPropertiesSetDummyInstanceAsNextStep()
        {
            // Arrange
            var indexerMock = new MockCanHaveNextPropertyStep<int>();
            indexerMock.SetNextStep<DummyPropertyStep<int>>().RecordBeforeCall(out var ledger);

            // Act
            indexerMock.Dummy();

            // Assert
            var item = Assert.Single(ledger);
            Assert.Same(DummyPropertyStep<int>.Instance, item);
        }
    }
}
