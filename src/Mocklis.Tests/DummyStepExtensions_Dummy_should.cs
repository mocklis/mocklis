// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyStepExtensions_Dummy_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Dummy;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class DummyStepExtensions_Dummy_should
    {
        [Fact]
        public void for_Events_set_Dummy_instance_as_next_step()
        {
            // Arrange
            var eventMock = new MockCanHaveNextEventStep<EventHandler>();
            eventMock.SetNextStep<DummyEventStep<EventHandler>>().RecordBeforeCall(out var ledger, s => s).Dummy();

            // Act
            eventMock.Dummy();

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Same(DummyEventStep<EventHandler>.Instance, ledger[0]);
        }

        [Fact]
        public void for_Indexers_set_Dummy_instance_as_next_step()
        {
            // Arrange
            var indexerMock = new MockCanHaveNextIndexerStep<int, string>();
            indexerMock.SetNextStep<DummyIndexerStep<int, string>>().RecordBeforeCall(out var ledger, s => s).Dummy();

            // Act
            indexerMock.Dummy();

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Same(DummyIndexerStep<int, string>.Instance, ledger[0]);
        }

        [Fact]
        public void for_Methods_set_Dummy_instance_as_next_step()
        {
            // Arrange
            var eventMock = new MockCanHaveNextMethodStep<int, string>();
            eventMock.SetNextStep<DummyMethodStep<int, string>>().RecordBeforeCall(out var ledger, s => s).Dummy();

            // Act
            eventMock.Dummy();

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Same(DummyMethodStep<int, string>.Instance, ledger[0]);
        }

        [Fact]
        public void for_Properties_set_Dummy_instance_as_next_step()
        {
            // Arrange
            var indexerMock = new MockCanHaveNextPropertyStep<int>();
            indexerMock.SetNextStep<DummyPropertyStep<int>>().RecordBeforeCall(out var ledger, s => s).Dummy();

            // Act
            indexerMock.Dummy();

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Same(DummyPropertyStep<int>.Instance, ledger[0]);
        }
    }
}
