// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Return' indexer step that returns a given value every time it's read.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public class ReturnIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly TValue _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="value">The value to be returned.</param>
        public ReturnIndexerStep(TValue value)
        {
            _value = value;
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation returns a given value every time.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return _value;
        }
    }
}
