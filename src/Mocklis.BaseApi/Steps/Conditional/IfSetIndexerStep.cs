// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfSetIndexerStep.cs">
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
    ///     Indexer step with alternative branch, where the alternative branch will be taken when a value is written to
    ///     the indexer, and the normal branch when a value is read.
    ///     Inherits from the <see cref="IfIndexerStepBase{TKey, TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IfIndexerStepBase{TKey, TValue}" />
    public class IfSetIndexerStep<TKey, TValue> : IfIndexerStepBase<TKey, TValue>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IfSetIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public IfSetIndexerStep(Action<IfBranchCaller> branch) : base(branch)
        {
        }

        /// <summary>
        ///     Called when a value is written to the indexer, and redirects the call onto the if branch.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            IfBranch.Set(mockInfo, key, value);
        }
    }
}
