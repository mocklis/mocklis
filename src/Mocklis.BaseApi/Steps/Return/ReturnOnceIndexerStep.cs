// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOnceIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Return' indexer step that returns given value once, and then forwards on reads.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public class ReturnOnceIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly TValue _value;
        private int _returnCount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnOnceIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="value">The value to return once.</param>
        public ReturnOnceIndexerStep(TValue value)
        {
            _value = value;
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation returns the given value once, and then forwards on reads.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            if (Interlocked.Exchange(ref _returnCount, 1) == 0)
            {
                return _value;
            }

            return base.Get(mockInfo, key);
        }
    }
}
