// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodStepWithNext.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Class that models a method step that can forward calls on to a next step. It is a common base class for
    ///     implementing new steps.
    ///     Implements the <see cref="IMethodStep{TParam, TResult}" /> interface.
    ///     Implements the <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IMethodStep{TParam, TResult}" />
    /// <seealso cref="ICanHaveNextMethodStep{TParam, TResult}" />
    public class MethodStepWithNext<TParam, TResult> : IMethodStep<TParam, TResult>, ICanHaveNextMethodStep<TParam, TResult>
    {
        /// <summary>
        ///     Gets the current next step.
        /// </summary>
        /// <value>The current next step.</value>
        protected IMethodStep<TParam, TResult>? NextStep { get; private set; }

        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        TStep ICanHaveNextMethodStep<TParam, TResult>.SetNextStep<TStep>(TStep step)
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
        ///     Can be overridden to provide a bespoke behaviour in a step. The default behaviour is to forward calls on to the
        ///     next
        ///     step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public virtual TResult Call(IMockInfo mockInfo, TParam param)
        {
            return NextStep.CallWithStrictnessCheckIfNull(mockInfo, param);
        }
    }
}
