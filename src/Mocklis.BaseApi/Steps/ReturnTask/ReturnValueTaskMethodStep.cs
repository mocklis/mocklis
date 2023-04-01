// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnValueTaskMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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
    ///     Class that represents a 'ReturnTask' method step that wraps the result from a subsequent step in a ValueTask.
    ///     Implements the <see cref="IMethodStep{TParam, TResult}" /> interface.
    ///     Implements the <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The return type that is wrapped in a ValueTask.</typeparam>
    public sealed class ReturnValueTaskMethodStep<TParam, TResult> : IMethodStep<TParam, ValueTask<TResult>>, ICanHaveNextMethodStep<TParam, TResult>
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
        ///     This implementation takes the response from the next step and wraps it in a ValueTask.
        ///     If the next step throws an exception the ValueTask returned is faulted or cancelled depending on the exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public ValueTask<TResult> Call(IMockInfo mockInfo, TParam param)
        {
            try
            {
                return new ValueTask<TResult>(NextStep.CallWithStrictnessCheckIfNull(mockInfo, param));
            }
            catch (OperationCanceledException c)
            {
                return new ValueTask<TResult>(Task.FromCanceled<TResult>(c.CancellationToken));
            }

            catch (Exception e)
            {
                return new ValueTask<TResult>(Task.FromException<TResult>(e));
            }
        }
    }

    /// <summary>
    ///     Class that represents a 'ReturnTask' method step that wraps the result from a subsequent step in a ValueTask.
    ///     Implements the <see cref="IMethodStep{TParam, TResult}" /> interface.
    ///     Implements the <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    public sealed class ReturnValueTaskMethodStep<TParam> : IMethodStep<TParam, ValueTask>, ICanHaveNextMethodStep<TParam, ValueTuple>
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
        ///     This implementation takes the response from the next step and wraps it in a ValueTask.
        ///     If the next step throws an exception the ValueTask returned is faulted or cancelled depending on the exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public ValueTask Call(IMockInfo mockInfo, TParam param)
        {
            try
            {
                NextStep.CallWithStrictnessCheckIfNull(mockInfo, param);
                return default;
            }
            catch (OperationCanceledException c)
            {
                return new ValueTask(Task.FromCanceled(c.CancellationToken));
            }
            catch (Exception e)
            {
                return new ValueTask(Task.FromException(e));
            }
        }
    }
}
