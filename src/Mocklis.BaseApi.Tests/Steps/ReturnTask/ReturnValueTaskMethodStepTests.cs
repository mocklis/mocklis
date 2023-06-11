// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnValueTaskMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.ReturnTask
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Mocklis.Core;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;
    using Xunit.Abstractions;

    #endregion

    public class ReturnValueTaskMethodStepTests
    {
        public MockValueTaskMethods MockValueTaskMethods { get; } = new MockValueTaskMethods();

        public ITestOutputHelper TestOutputHelper { get; }

        public ReturnValueTaskMethodStepTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task WrapsVoidReturnValue()
        {
            MockValueTaskMethods.ReturnValueTask.ReturnTask().Dummy();
            var task = ((IValueTaskMethods)MockValueTaskMethods).ReturnValueTask();

            Assert.False(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.False(task.IsFaulted);

            await task;
        }

        [Fact]
        public async Task WrapsIntReturnValue()
        {
            MockValueTaskMethods.ReturnValueTaskInt.ReturnTask().Return(5);
            var task = ((IValueTaskMethods)MockValueTaskMethods).ReturnValueTaskInt();

            Assert.False(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.False(task.IsFaulted);

            var result = await task;
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task WrapsCancellation()
        {
            var source = new CancellationTokenSource();
            source.Cancel();

            MockValueTaskMethods.ReturnValueTask.ReturnTask().Action(_ => source.Token.ThrowIfCancellationRequested());
            var task = ((IValueTaskMethods)MockValueTaskMethods).ReturnValueTask();
            Assert.True(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.False(task.IsFaulted);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        }

        [Fact]
        public async Task WrapsCancellation2()
        {
            var source = new CancellationTokenSource();
            source.Cancel();

            MockValueTaskMethods.ReturnValueTaskInt.ReturnTask().Func(_ =>
            {
                source.Token.ThrowIfCancellationRequested();
                return 5;
            });

            var task = ((IValueTaskMethods)MockValueTaskMethods).ReturnValueTaskInt();
            Assert.True(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.False(task.IsFaulted);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        }

        [Fact]
        public async Task WrapsException()
        {
            MockValueTaskMethods.ReturnValueTask.ReturnTask().Throw(() => new InvalidOperationException("Aha"));
            var task = ((IValueTaskMethods)MockValueTaskMethods).ReturnValueTask();
            Assert.False(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.True(task.IsFaulted);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await task);
            Assert.Equal("Aha", ex.Message);
        }

        [Fact]
        public async Task WrapsException2()
        {
            var source = new CancellationTokenSource();
            source.Cancel();

            MockValueTaskMethods.ReturnValueTaskInt.ReturnTask().Throw(() => new InvalidOperationException("Aha"));

            var task = ((IValueTaskMethods)MockValueTaskMethods).ReturnValueTaskInt();
            Assert.False(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.True(task.IsFaulted);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await task);
            Assert.Equal("Aha", ex.Message);
        }

        [Fact]
        public void SetNextStepShouldNotAcceptNull()
        {
            ICanHaveNextMethodStep<int, int> step = new ReturnValueTaskMethodStep<int, int>();
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IMethodStep<int, int>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void SetNextStepShouldNotAcceptNull2()
        {
            ICanHaveNextMethodStep<int, ValueTuple> step = new ReturnValueTaskMethodStep<int>();
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IMethodStep<int, ValueTuple>)null!));
            Assert.Equal("step", exception.ParamName);
        }
    }
}
