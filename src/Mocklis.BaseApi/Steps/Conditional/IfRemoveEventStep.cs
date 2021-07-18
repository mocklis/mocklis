// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfRemoveEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Event step with alternative branch, where the alternative branch will be taken when event handlers are removed,
    ///     and the normal branch when they are added.
    ///     Inherits from the <see cref="IfEventStepBase{THandler}" /> class.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="IfEventStepBase{THandler}" />
    public class IfRemoveEventStep<THandler> : IfEventStepBase<THandler> where THandler : Delegate
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IfRemoveEventStep{THandler}" /> class.
        /// </summary>
        /// <param name="branch">Action used to populate the alternative branch with steps.</param>
        public IfRemoveEventStep(Action<IfBranchCaller> branch) :
            base(branch)
        {
        }

        /// <summary>
        ///     Called when an event handler is being removed from the mocked event. This implementation redirects the call onto
        ///     the alternative branch.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public override void Remove(IMockInfo mockInfo, THandler? value)
        {
            IfBranch.Remove(mockInfo, value);
        }
    }
}
