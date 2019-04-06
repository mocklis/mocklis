// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetFuncIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'GetFunc' indexer step.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public class GetFuncIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _func;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetFuncIndexerStep{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="func">A function used to create a value when the indexer is read from.</param>
        public GetFuncIndexerStep(Func<TKey, TValue> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation evaluates the function with the indexer key as parameter and returns the result.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return _func(key);
        }
    }
}
