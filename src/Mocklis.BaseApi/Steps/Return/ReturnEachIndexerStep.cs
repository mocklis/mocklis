// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Return' indexer step that returns given set of values one-by-one, and then forwards on
    ///     reads.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public class ReturnEachIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly object _lockObject = new object();
        private IEnumerator<TValue>? _values;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnEachIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="values">The values to be returned one-by-one.</param>
        public ReturnEachIndexerStep(IEnumerable<TValue> values)
        {
            _values = (values ?? throw new ArgumentNullException(nameof(values))).GetEnumerator();
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation returns the values provided one-by-one, and then forwards on reads.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            if (_values == null)
            {
                return base.Get(mockInfo, key);
            }

            lock (_lockObject)
            {
                if (_values != null)
                {
                    if (_values.MoveNext())
                    {
                        return _values.Current;
                    }

                    _values.Dispose();
                    _values = null;
                }
            }

            return base.Get(mockInfo, key);
        }
    }
}
