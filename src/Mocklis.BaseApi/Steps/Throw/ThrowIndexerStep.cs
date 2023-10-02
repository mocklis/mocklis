// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Throw' indexer step, that will throw an exception whenever a value is read from or written
    ///     to the indexer.
    ///     Implements the <see cref="IIndexerStep{TKey,TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IIndexerStep{TKey, TValue}" />
    public class ThrowIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        private readonly Func<object, TKey, Exception> _exceptionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThrowIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="exceptionFactory">
        ///     A Func that creates the exception to be thrown. Takes the mocked instance and indexer
        ///     key as parameters.
        /// </param>
        public ThrowIndexerStep(Func<object, TKey, Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        /// <summary>
        ///     Called when a value is read from the indexer. This implementation creates and throws an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public TValue Get(IMockInfo mockInfo, TKey key)
        {
            throw _exceptionFactory(mockInfo.MockInstance, key);
        }

        /// <summary>
        ///     Called when a value is written to the indexer. This implementation creates and throws an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            throw _exceptionFactory(mockInfo.MockInstance, key);
        }
    }
}
