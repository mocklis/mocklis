// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStepBase.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
    ///     Base class for conditional property steps with an alternative branch and the ability to
    ///     rejoin the normal branch from the alternative branch.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public abstract class IfPropertyStepBase<TValue> : PropertyStepWithNext<TValue>
    {
        /// <summary>
        ///     Starting point for the alternative path of a conditional property step. This class cannot be inherited.
        ///     Implements the <see cref="IPropertyStep{TValue}" /> interface.
        ///     Implements the <see cref="ICanHaveNextPropertyStep{TValue}" /> interface.
        /// </summary>
        /// <seealso cref="IPropertyStep{TValue}" />
        /// <seealso cref="ICanHaveNextPropertyStep{TValue}" />
        public sealed class IfBranchCaller : IPropertyStep<TValue>, ICanHaveNextPropertyStep<TValue>
        {
            private IPropertyStep<TValue> _nextStep = MissingPropertyStep<TValue>.Instance;

            /// <summary>
            ///     Replaces the current 'next' step with a new step.
            /// </summary>
            /// <typeparam name="TStep">The actual type of the new step.</typeparam>
            /// <param name="step">The new step.</param>
            /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
            TStep ICanHaveNextPropertyStep<TValue>.SetNextStep<TStep>(TStep step)
            {
                if (step == null)
                {
                    throw new ArgumentNullException(nameof(step));
                }

                _nextStep = step;
                return step;
            }

            /// <summary>
            ///     Called when a value is read from the property.
            /// </summary>
            /// <param name="mockInfo">Information about the mock through which the value is read.</param>
            /// <returns>The value being read.</returns>
            TValue IPropertyStep<TValue>.Get(IMockInfo mockInfo)
            {
                return _nextStep.Get(mockInfo);
            }

            /// <summary>
            ///     Called when a value is written to the property.
            /// </summary>
            /// <param name="mockInfo">Information about the mock through which the value is written.</param>
            /// <param name="value">The value being written.</param>
            void IPropertyStep<TValue>.Set(IMockInfo mockInfo, TValue value)
            {
                _nextStep.Set(mockInfo, value);
            }

            /// <summary>
            ///     Gets a step that can be used to rejoin the normal ('else') branch.
            /// </summary>
            public IPropertyStep<TValue> ElseBranch { get; }

            /// <summary>
            ///     Initializes a new instance of the <see cref="IfBranchCaller" /> class.
            /// </summary>
            /// <param name="ifPropertyStep">The if step whose 'default' set of steps will constitute 'else' branch.</param>
            public IfBranchCaller(IfPropertyStepBase<TValue> ifPropertyStep)
            {
                ElseBranch = new ElseBranchRejoiner(ifPropertyStep);
            }
        }

        private sealed class ElseBranchRejoiner : IPropertyStep<TValue>
        {
            private readonly IfPropertyStepBase<TValue> _ifPropertyStep;

            public ElseBranchRejoiner(IfPropertyStepBase<TValue> ifPropertyStep)
            {
                _ifPropertyStep = ifPropertyStep;
            }

            public TValue Get(IMockInfo mockInfo)
            {
                // Call directly to next step thus bypassing the condition check.
                return _ifPropertyStep.NextStep.Get(mockInfo);
            }

            public void Set(IMockInfo mockInfo, TValue value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifPropertyStep.NextStep.Set(mockInfo, value);
            }
        }

        /// <summary>
        ///     Gets the alternative branch.
        /// </summary>
        protected IPropertyStep<TValue> IfBranch { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfPropertyStepBase{TValue}" /> class.
        /// </summary>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        protected IfPropertyStepBase(Action<IfBranchCaller> branch)
        {
            var ifBranch = new IfBranchCaller(this);
            IfBranch = ifBranch;
            branch?.Invoke(ifBranch);
        }
    }
}
