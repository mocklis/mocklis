// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingStepExtensions_Missing_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class MissingStepExtensions_Missing_should
    {
        [Fact]
        public void for_Events_set_Missing_instance_as_next_step()
        {
            // Arrange
            var eventMock = new MockCanHaveNextEventStep<EventHandler>();
            eventMock.SetNextStep<MissingEventStep<EventHandler>>().RecordBeforeCall(out var ledger, s => s).Dummy();

            // Act
            eventMock.Missing();

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Same(MissingEventStep<EventHandler>.Instance, ledger[0]);
        }

        [Fact]
        public void for_Indexers_set_Missing_instance_as_next_step()
        {
            // Arrange
            var indexerMock = new MockCanHaveNextIndexerStep<int, string>();
            indexerMock.SetNextStep<MissingIndexerStep<int, string>>().RecordBeforeCall(out var ledger, s => s).Dummy();

            // Act
            indexerMock.Missing();

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Same(MissingIndexerStep<int, string>.Instance, ledger[0]);
        }

        [Fact]
        public void for_Methods_set_Missing_instance_as_next_step()
        {
            // Arrange
            var eventMock = new MockCanHaveNextMethodStep<int, string>();
            eventMock.SetNextStep<MissingMethodStep<int, string>>().RecordBeforeCall(out var ledger, s => s).Dummy();

            // Act
            eventMock.Missing();

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Same(MissingMethodStep<int, string>.Instance, ledger[0]);
        }

        [Fact]
        public void for_Properties_set_Missing_instance_as_next_step()
        {
            // Arrange
            var indexerMock = new MockCanHaveNextPropertyStep<int>();
            indexerMock.SetNextStep<MissingPropertyStep<int>>().RecordBeforeCall(out var ledger, s => s).Dummy();

            // Act
            indexerMock.Missing();

            // Assert
            Assert.Equal(1, ledger.Count);
            Assert.Same(MissingPropertyStep<int>.Instance, ledger[0]);
        }
    }
}
