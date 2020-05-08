// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnTaskMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.ReturnTask
{
    #region Using Directives

    using System;
    using System.Threading.Tasks;
    using Mocklis.Core;

    #endregion

#pragma warning disable CA1031 // Do not catch general exception types

    /// <summary>
    ///     Class that represents a 'ReturnTask' method step that wraps the result from a subsequent step in a Task.
    ///     Implements the <see cref="IMethodStep{TParam, TResult}" /> interface.
    ///     Implements the <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The return type that is wrapped in a Task.</typeparam>
    public sealed class ReturnTaskMethodStep<TParam, TResult> : IMethodStep<TParam, Task<TResult>>, ICanHaveNextMethodStep<TParam, TResult>
    {
        /// <summary>
        ///     Gets the current next step.
        /// </summary>
        /// <value>The current next step.</value>
        private IMethodStep<TParam, TResult>? NextStep { get; set; }

        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        public TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, TResult>
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation takes the return value from the next step and wraps it in a Task.
        ///     If the next step throws an exception the Task returned is faulted or cancelled depending on the exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result, wrapped in a Task.</returns>
        public Task<TResult> Call(IMockInfo mockInfo, TParam param)
        {
#if NET45
            var tcl = new TaskCompletionSource<TResult>();
            try
            {
                var result = NextStep.CallWithStrictnessCheckIfNull(mockInfo, param);
                tcl.SetResult(result);
            }
            catch (OperationCanceledException)
            {
                tcl.SetCanceled();
            }
            catch (Exception e)
            {
                tcl.SetException(e);
            }
            return tcl.Task;
#else
            try
            {
                return Task.FromResult(NextStep.CallWithStrictnessCheckIfNull(mockInfo, param));
            }
            catch (OperationCanceledException c)
            {
                return Task.FromCanceled<TResult>(c.CancellationToken);
            }
            catch (Exception e)
            {
                return Task.FromException<TResult>(e);
            }
#endif
        }
    }

    /// <summary>
    ///     Class that represents a 'ReturnTask' method step that wraps the result from a subsequent step in a Task.
    ///     Implements the <see cref="IMethodStep{TParam, TResult}" /> interface.
    ///     Implements the <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    public sealed class ReturnTaskMethodStep<TParam> : IMethodStep<TParam, Task>, ICanHaveNextMethodStep<TParam, ValueTuple>
    {
        /// <summary>
        ///     Gets the current next step.
        /// </summary>
        /// <value>The current next step.</value>
        private IMethodStep<TParam, ValueTuple>? NextStep { get; set; }

        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        public TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, ValueTuple>
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation takes the return value from the next step and wraps it in a Task.
        ///     If the next step throws an exception the Task returned is faulted or cancelled depending on the exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result, wrapped in a Task.</returns>
        public Task Call(IMockInfo mockInfo, TParam param)
        {
#if NET45
            var tcl = new TaskCompletionSource<bool>();
            try
            {
                NextStep.CallWithStrictnessCheckIfNull(mockInfo, param);
                tcl.SetResult(false);
            }
            catch (OperationCanceledException)
            {
                tcl.SetCanceled();
            }
            catch (Exception e)
            {
                tcl.SetException(e);
            }
            return tcl.Task;
#else
            try
            {
                return Task.FromResult(NextStep.CallWithStrictnessCheckIfNull(mockInfo, param));
            }
            catch (OperationCanceledException c)
            {
                return Task.FromCanceled(c.CancellationToken);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
#endif
        }
    }
}
