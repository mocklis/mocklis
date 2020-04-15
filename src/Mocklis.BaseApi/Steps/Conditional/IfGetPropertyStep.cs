// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfGetPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Property step with alternative branch, where the alternative branch will be taken when a value is read from
    ///     the property, and the normal branch when a value is written.
    ///     Inherits from the <see cref="IfPropertyStepBase{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="IfPropertyStepBase{TValue}" />
    public class IfGetPropertyStep<TValue> : IfPropertyStepBase<TValue>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IfGetPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public IfGetPropertyStep(Action<IfBranchCaller> branch) : base(branch)
        {
        }

        /// <summary>
        ///     Called when a value is read from the property, and redirects calls onto the if branch.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            return IfBranch.Get(mockInfo);
        }
    }
}
