// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GateMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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

    /// <summary>
    ///     Class that represents a 'Gate' method step, that will complete a Task the first time the method has been called.
    ///     Inherits from the <see cref="MethodStepWithNext{TParam,TResult}" /> class.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MethodStepWithNext{TParam, TResult}" />
    public class GateMethodStep<TParam, TResult> : MethodStepWithNext<TParam, TResult>
    {
        private readonly TaskCompletionSource<TResult> _taskCompletionSource;

        /// <summary>
        ///     Gets the task that tracks whether the method has been called..
        /// </summary>
        public Task<TResult> Task => _taskCompletionSource.Task;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GateMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="cancellationToken">An optional cancellation token that can be used to cancel the task.</param>
        public GateMethodStep(CancellationToken cancellationToken = default)
        {
            _taskCompletionSource = new TaskCompletionSource<TResult>();
            cancellationToken.Register(() => _taskCompletionSource.TrySetCanceled());
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation will complete the task when the method returns, or fail the task if the method throws an
        ///     exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            try
            {
                var result = base.Call(mockInfo, param);
                _taskCompletionSource.TrySetResult(default);
                return result;
            }
            catch (Exception e)
            {
                _taskCompletionSource.TrySetException(e);
                throw;
            }
        }
    }
}
