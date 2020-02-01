// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfGetIndexerStep.cs">
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
    ///     Indexer step with alternative branch, where the alternative branch will be taken when a value is read from
    ///     the indexer, and the normal branch when a value is written.
    ///     Inherits from the <see cref="IfIndexerStepBase{TKey, TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IfIndexerStepBase{TKey, TValue}" />
    public class IfGetIndexerStep<TKey, TValue> : IfIndexerStepBase<TKey, TValue>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IfGetIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public IfGetIndexerStep(Action<IfBranchCaller> branch) : base(branch)
        {
        }

        /// <summary>
        ///     Called when a value is read from the indexer, and redirects the call onto the if branch.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return IfBranch.Get(mockInfo, key);
        }
    }
}
