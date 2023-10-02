// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Event step with an alternative set of steps that can be chosen given the provided conditions.
    ///     Inherits from the <see cref="IfEventStepBase{THandler}" /> class.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="IfEventStepBase{THandler}" />
    public class IfEventStep<THandler> : IfEventStepBase<THandler> where THandler : Delegate
    {
        private readonly Func<THandler?, bool>? _addCondition;
        private readonly Func<THandler?, bool>? _removeCondition;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfEventStep{THandler}" /> class.
        /// </summary>
        /// <param name="addCondition">
        ///     A condition evaluated when an event handler is added. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="removeCondition">
        ///     A condition evaluated when an event handler is removed. If <c>true</c>, the alternative
        ///     branch is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public IfEventStep(Func<THandler?, bool>? addCondition, Func<THandler?, bool>? removeCondition,
            Action<IfBranchCaller> branch) :
            base(branch)
        {
            _addCondition = addCondition;
            _removeCondition = removeCondition;
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        ///     This implementation will select the alternative branch if the add condition evaluates to <c>true</c>.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public override void Add(IMockInfo mockInfo, THandler? value)
        {
            if (_addCondition?.Invoke(value) ?? false)
            {
                IfBranch.Add(mockInfo, value);
            }
            else
            {
                base.Add(mockInfo, value);
            }
        }

        /// <summary>
        ///     Called when an event handler is being removed from the mocked event.
        ///     This implementation will select the alternative branch if the remove condition evaluates to <c>true</c>.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public override void Remove(IMockInfo mockInfo, THandler? value)
        {
            if (_removeCondition?.Invoke(value) ?? false)
            {
                IfBranch.Remove(mockInfo, value);
            }
            else
            {
                base.Remove(mockInfo, value);
            }
        }
    }
}
