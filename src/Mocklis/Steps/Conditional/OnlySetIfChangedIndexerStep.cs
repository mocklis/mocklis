// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlySetIfChangedIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Indexer step that will only forward a write to the next step if the value is different from
    ///     what can currently be read from the next step.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public class OnlySetIfChangedIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private IEqualityComparer<TValue> Comparer { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OnlySetIfChangedIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">
        ///     An optional <see cref="IEqualityComparer{TValue}" /> that is used to determine whether the new
        ///     value is different from the current one.
        /// </param>
        public OnlySetIfChangedIndexerStep(IEqualityComparer<TValue> comparer = null)
        {
            Comparer = comparer ?? EqualityComparer<TValue>.Default;
        }

        /// <summary>
        ///     Called when a value is written to the indexer. This implementation first gets the current value, and only calls
        ///     next if this value differs from the one that is to be written.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            if (!Comparer.Equals(NextStep.Get(mockInfo, key), value))
            {
                base.Set(mockInfo, key, value);
            }
        }
    }
}
