// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnTaskMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
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

    #endregion

    public class ReturnTaskMethodStepTests
    {
        public MockTaskMethods MockTaskMethods { get; } = new MockTaskMethods();

        [Fact]
        public async Task WrapsVoidReturnValue()
        {
            MockTaskMethods.ReturnTask.ReturnTask().Dummy();
            var task = ((ITaskMethods)MockTaskMethods).ReturnTask();

            Assert.False(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.False(task.IsFaulted);

            await task;
        }

        [Fact]
        public async Task WrapsIntReturnValue()
        {
            MockTaskMethods.ReturnTaskInt.ReturnTask().Return(5);
            var task = ((ITaskMethods)MockTaskMethods).ReturnTaskInt();

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

            MockTaskMethods.ReturnTask.ReturnTask().Action(_ => source.Token.ThrowIfCancellationRequested());
            var task = ((ITaskMethods)MockTaskMethods).ReturnTask();
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

            MockTaskMethods.ReturnTaskInt.ReturnTask().Func(_ =>
            {
                source.Token.ThrowIfCancellationRequested();
                return 5;
            });

            var task = ((ITaskMethods)MockTaskMethods).ReturnTaskInt();
            Assert.True(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.False(task.IsFaulted);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        }

        [Fact]
        public async Task WrapsException()
        {
            MockTaskMethods.ReturnTask.ReturnTask().Throw(() => new InvalidOperationException("Aha"));
            var task = ((ITaskMethods)MockTaskMethods).ReturnTask();
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

            MockTaskMethods.ReturnTaskInt.ReturnTask().Throw(() => new InvalidOperationException("Aha"));

            var task = ((ITaskMethods)MockTaskMethods).ReturnTaskInt();
            Assert.False(task.IsCanceled);
            Assert.True(task.IsCompleted);
            Assert.True(task.IsFaulted);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await task);
            Assert.Equal("Aha", ex.Message);
        }

        [Fact]
        public void SetNextStepShouldNotAcceptNull()
        {
            ICanHaveNextMethodStep<int, int> step = new ReturnTaskMethodStep<int, int>();
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IMethodStep<int, int>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void SetNextStepShouldNotAcceptNull2()
        {
            ICanHaveNextMethodStep<int, ValueTuple> step = new ReturnTaskMethodStep<int>();
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IMethodStep<int, ValueTuple>)null!));
            Assert.Equal("step", exception.ParamName);
        }
    }
}
