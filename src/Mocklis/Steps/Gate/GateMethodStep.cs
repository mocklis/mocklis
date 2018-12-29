// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GateMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Gate
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Mocklis.Core;

    #endregion

    public class GateMethodStep<TParam, TResult> : MethodStepWithNext<TParam, TResult>, IGate
    {
        private readonly TaskCompletionSource<ValueTuple> _taskCompletionSource;

        public Task GatePassed => _taskCompletionSource.Task;

        public GateMethodStep(CancellationToken cancellationToken = default)
        {
            _taskCompletionSource = new TaskCompletionSource<ValueTuple>();
            cancellationToken.Register(() => _taskCompletionSource.TrySetCanceled(cancellationToken));
        }

        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            var result = base.Call(mockInfo, param);
            _taskCompletionSource.TrySetResult(default);
            return result;
        }
    }
}
