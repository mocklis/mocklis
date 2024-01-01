// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStepBase.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    /*
     * See comment on IfEventStepBase class.
     */

    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Missing;

    #endregion

    /// <summary>
    ///     Base class for conditional method steps with an alternative branch and the ability to
    ///     rejoin the normal branch from the alternative branch.
    ///     Inherits from the <see cref="MethodStepWithNext{TParam,TResult}" /> class.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MethodStepWithNext{TParam, TResult}" />
    public abstract class IfMethodStepBase<TParam, TResult> : MethodStepWithNext<TParam, TResult>
    {
        /// <summary>
        ///     Starting point for the alternative path of a conditional method step. This class cannot be inherited.
        ///     Implements the <see cref="IMethodStep{TParam, TResult}" /> interface.
        ///     Implements the <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> interface.
        /// </summary>
        /// <seealso cref="IMethodStep{TParam, TResult}" />
        /// <seealso cref="ICanHaveNextMethodStep{TParam, TResult}" />
        public sealed class IfBranchCaller : IMethodStep<TParam, TResult>, ICanHaveNextMethodStep<TParam, TResult>
        {
            private IMethodStep<TParam, TResult> _nextStep = MissingMethodStep<TParam, TResult>.Instance;

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

                _nextStep = step;
                return step;
            }

            /// <summary>
            ///     Called when the mocked method is called.
            /// </summary>
            /// <param name="mockInfo">Information about the mock through which the method is called.</param>
            /// <param name="param">The parameters used.</param>
            /// <returns>The returned result.</returns>
            TResult IMethodStep<TParam, TResult>.Call(IMockInfo mockInfo, TParam param)
            {
                return _nextStep.CallWithStrictnessCheckIfNull(mockInfo, param);
            }

            /// <summary>
            ///     Gets a step that can be used to rejoin the normal ('else') branch.
            /// </summary>
            public IMethodStep<TParam, TResult> ElseBranch { get; }

            /// <summary>
            ///     Initializes a new instance of the <see cref="IfBranchCaller" /> class.
            /// </summary>
            /// <param name="ifMethodStep">The if step whose 'default' set of steps will constitute 'else' branch.</param>
            public IfBranchCaller(IfMethodStepBase<TParam, TResult> ifMethodStep)
            {
                ElseBranch = new ElseBranchRejoiner(ifMethodStep);
            }
        }

        private sealed class ElseBranchRejoiner : IMethodStep<TParam, TResult>
        {
            private readonly IfMethodStepBase<TParam, TResult> _ifMethodStep;

            public ElseBranchRejoiner(IfMethodStepBase<TParam, TResult> ifMethodStep)
            {
                _ifMethodStep = ifMethodStep;
            }

            public TResult Call(IMockInfo mockInfo, TParam param)
            {
                // Call directly to next step thus bypassing the condition check.
                return _ifMethodStep.NextStep.CallWithStrictnessCheckIfNull(mockInfo, param);
            }
        }

        /// <summary>
        ///     Gets the alternative branch.
        /// </summary>
        protected IMethodStep<TParam, TResult> IfBranch { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfMethodStepBase{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        protected IfMethodStepBase(Action<IfBranchCaller> branch)
        {
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch));
            }

            var ifBranch = new IfBranchCaller(this);
            IfBranch = ifBranch;
            branch.Invoke(ifBranch);
        }
    }
}
