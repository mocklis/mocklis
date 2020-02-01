// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GateMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Experimental.Tests.Steps
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Mocklis.Experimental.Tests.Interfaces;
    using Mocklis.Experimental.Tests.Mocks;
    using Xunit;

    #endregion

    public class GateMethodStepTests
    {
        private MockCalculator MockCalculator { get; } = new MockCalculator();
        private ICalculator Calculator { get; }

        public GateMethodStepTests()
        {
            Calculator = MockCalculator;
        }

        [Fact]
        public async Task GateTaskCanComplete()
        {
            MockCalculator.Calculate.Gate(out var task).Return(25);

            Assert.Equal(TaskStatus.WaitingForActivation, task.Status);
            Calculator.Calculate(5);
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);

            var result = await task;
            Assert.Equal(25, result);
        }

        [Fact]
        public async Task GateTaskCanFail()
        {
            MockCalculator.Calculate.Gate(out var task).Throw(a => new Exception("Failed miserably with input " + a));

            Assert.Equal(TaskStatus.WaitingForActivation, task.Status);
            Assert.Throws<Exception>(() => Calculator.Calculate(5));
            Assert.Equal(TaskStatus.Faulted, task.Status);

            var ex = await Assert.ThrowsAsync<Exception>(async () => await task);
            Assert.Equal("Failed miserably with input 5", ex.Message);
        }

        [Fact]
        public async Task GateTaskCanBeCancelled()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            MockCalculator.Calculate.Gate(out var task, source.Token);

            Assert.Equal(TaskStatus.WaitingForActivation, task.Status);
            source.Cancel();
            Assert.Equal(TaskStatus.Canceled, task.Status);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        }
    }
}
