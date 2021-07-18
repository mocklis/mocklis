// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfIndexerStep.cs">
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
    ///     Indexer step with an alternative set of steps that can be chosen given the provided conditions.
    ///     Inherits from the <see cref="IfIndexerStepBase{TKey, TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IfIndexerStepBase{TKey, TValue}" />
    public class IfIndexerStep<TKey, TValue> : IfIndexerStepBase<TKey, TValue>
    {
        private readonly Func<TKey, bool>? _getCondition;
        private readonly Func<TKey, TValue, bool>? _setCondition;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="getCondition">A condition evaluated when a value is read. If <c>true</c>, the alternative branch is taken.</param>
        /// <param name="setCondition">
        ///     A condition evaluated when a value is written. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public IfIndexerStep(Func<TKey, bool>? getCondition, Func<TKey, TValue, bool>? setCondition,
            Action<IfBranchCaller> branch) : base(branch)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation will select the alternative branch if the get condition evaluates to <c>true</c>.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            if (_getCondition?.Invoke(key) ?? false)
            {
                return IfBranch.Get(mockInfo, key);
            }

            return base.Get(mockInfo, key);
        }

        /// <summary>
        ///     Called when a value is written to the indexer.
        ///     This implementation will select the alternative branch if the set condition evaluates to <c>true</c>.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            if (_setCondition?.Invoke(key, value) ?? false)
            {
                IfBranch.Set(mockInfo, key, value);
            }
            else
            {
                base.Set(mockInfo, key, value);
            }
        }
    }
}
