// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnTaskStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.Threading.Tasks;
    using Mocklis.Core;
    using Mocklis.Steps.ReturnTask;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'return task' steps to an existing mock or step.
    /// </summary>
    public static class ReturnTaskStepExtensions
    {
        /// <summary>
        ///     Introduces a step that wraps the return value of the next step in a Task.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type (without the Task).</typeparam>
        /// <param name="caller">The mock or step to which this 'return task' step is added.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> ReturnTask<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, Task<TResult>> caller)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            return caller.SetNextStep(new ReturnTaskMethodStep<TParam, TResult>());
        }

        /// <summary>
        ///     Introduces a step that wraps the return value of the next step in a Task.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <param name="caller">The mock or step to which this 'return task' step is added.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, ValueTuple> ReturnTask<TParam>(
            this ICanHaveNextMethodStep<TParam, Task> caller)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            return caller.SetNextStep(new ReturnTaskMethodStep<TParam>());
        }

        /// <summary>
        ///     Introduces a step that wraps the return value of the next step in a ValueTask.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type (without the ValueTask).</typeparam>
        /// <param name="caller">The mock or step to which this 'return task' step is added.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> ReturnTask<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, ValueTask<TResult>> caller)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            return caller.SetNextStep(new ReturnValueTaskMethodStep<TParam, TResult>());
        }

        /// <summary>
        ///     Introduces a step that wraps the return value of the next step in a ValueTask.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <param name="caller">The mock or step to which this 'return task' step is added.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, ValueTuple> ReturnTask<TParam>(
            this ICanHaveNextMethodStep<TParam, ValueTask> caller)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            return caller.SetNextStep(new ReturnValueTaskMethodStep<TParam>());
        }
    }
}
