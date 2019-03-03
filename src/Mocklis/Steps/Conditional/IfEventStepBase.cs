// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfEventStepBase.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    /*
     * This class looks like it's more complicated than it needs to be. The key is that
     * we need to have three separate implementations of the IEventStep<THandler> interface:
     * 1. The step itself (IfEventStepBase and derived classes)
     * 2. one that represents the if-branch (IfBranchCaller), so that classes deriving from
     *    IfEventStepBase can direct calls to it.
     * 3. one that can be used to rejoin the original branch (ElseBranchRejoiner). This needs
     *    to be separate from 1., since we need to bypass the if-logic if we use it.
     *
     * ElseBranchRejoiner needs to be an inner class to IfEventStepBase to have visibility
     * on its NextStep property.
     *
     * IfBranchCaller needs to be public, since it needs to expose ICanHaveNextEventStep<THandler>
     * and the ElseBranch Property through the IfBranch property of the IfEventStepBase class.
     * If the latter returned ICanHaveNextEventStep we wouldn't see the ElseBranch, and if it
     * returned an interface incorporating both then the extension methods that connect up
     * further steps would stop working. It needs to be an inner class to IfBranchCaller to
     * be able to construct the ElseBranchRejoiner for its ElseBranch property.
     *
     * Lastly, while the IfBranchCaller has much code in common with EventStepWithNext
     * we prefer to have an explicit declaration of the IEventStep<THandler> interface,
     * otherwise the Add and Remove methods would show up in intellisense when setting up
     * the steps for the if branch.
     *
     * All in all this is the simplest thing that could work. Pity it isn't particularly simple.
     */

    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Missing;

    #endregion

    /// <summary>
    ///     Base class for conditional event steps with an alternative branch and the ability to
    ///     rejoin the normal branch from the alternative branch.
    ///     Inherits from the <see cref="EventStepWithNext{THandler}" /> class.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="EventStepWithNext{THandler}" />
    public abstract class IfEventStepBase<THandler> : EventStepWithNext<THandler> where THandler : Delegate
    {
        /// <summary>
        ///     Starting point for the alternative path of a conditional event step. This class cannot be inherited.
        ///     Implements the <see cref="IEventStep{THandler}" /> interface.
        ///     Implements the <see cref="ICanHaveNextEventStep{THandler}" /> interface.
        /// </summary>
        /// <seealso cref="IEventStep{THandler}" />
        /// <seealso cref="ICanHaveNextEventStep{THandler}" />
        public sealed class IfBranchCaller : IEventStep<THandler>, ICanHaveNextEventStep<THandler>
        {
            private IEventStep<THandler> _nextStep = MissingEventStep<THandler>.Instance;

            /// <summary>
            ///     Replaces the current 'next' step with a new step.
            /// </summary>
            /// <typeparam name="TStep">The actual type of the new step.</typeparam>
            /// <param name="step">The new step.</param>
            /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
            TStep ICanHaveNextEventStep<THandler>.SetNextStep<TStep>(TStep step)
            {
                if (step == null)
                {
                    throw new ArgumentNullException(nameof(step));
                }

                _nextStep = step;
                return step;
            }

            /// <summary>
            ///     Called when an event handler is being added.
            /// </summary>
            /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
            /// <param name="value">The event handler that is being added.</param>
            void IEventStep<THandler>.Add(IMockInfo mockInfo, THandler value)
            {
                _nextStep.Add(mockInfo, value);
            }

            /// <summary>
            ///     Called when an event handler is being removed.
            /// </summary>
            /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
            /// <param name="value">The event handler that is being removed.</param>
            void IEventStep<THandler>.Remove(IMockInfo mockInfo, THandler value)
            {
                _nextStep.Remove(mockInfo, value);
            }

            /// <summary>
            ///     Gets a step that can be used to rejoin the normal ('else') branch.
            /// </summary>
            public IEventStep<THandler> ElseBranch { get; }

            /// <summary>
            ///     Initializes a new instance of the <see cref="IfBranchCaller" /> class.
            /// </summary>
            /// <param name="ifEventStep">The if step whose 'default' set of steps will constitute 'else' branch.</param>
            public IfBranchCaller(IfEventStepBase<THandler> ifEventStep)
            {
                ElseBranch = new ElseBranchRejoiner(ifEventStep);
            }
        }

        private sealed class ElseBranchRejoiner : IEventStep<THandler>
        {
            private readonly IfEventStepBase<THandler> _ifEventStep;

            public ElseBranchRejoiner(IfEventStepBase<THandler> ifEventStep)
            {
                _ifEventStep = ifEventStep;
            }

            public void Add(IMockInfo mockInfo, THandler value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifEventStep.NextStep.Add(mockInfo, value);
            }

            public void Remove(IMockInfo mockInfo, THandler value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifEventStep.NextStep.Remove(mockInfo, value);
            }
        }

        /// <summary>
        ///     Gets the alternative branch.
        /// </summary>
        protected IEventStep<THandler> IfBranch { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfEventStepBase{THandler}" /> class.
        /// </summary>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        protected IfEventStepBase(Action<IfBranchCaller> branch)
        {
            var ifBranch = new IfBranchCaller(this);
            IfBranch = ifBranch;
            branch?.Invoke(ifBranch);
        }
    }
}
