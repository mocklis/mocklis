// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Log' indexer step. This class cannot be inherited.
    ///     Implements the <see cref="IndexerStepWithNext{TKey,TValue}" />
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public sealed class LogIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly ILogContext _logContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="logContext">The log context used to write log lines.</param>
        public LogIndexerStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation logs befor and after the value has been read, along with any exceptions thrown.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            _logContext.LogBeforeIndexerGet(mockInfo, key);
            TValue result;
            try
            {
                result = base.Get(mockInfo, key);
            }
            catch (Exception exception)
            {
                _logContext.LogIndexerGetException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterIndexerGet(mockInfo, result);
            return result;
        }

        /// <summary>
        ///     Called when a value is written to the indexer.
        ///     This implementation logs befor and after the value has been written, along with any exceptions thrown.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            _logContext.LogBeforeIndexerSet(mockInfo, key, value);
            try
            {
                base.Set(mockInfo, key, value);
            }
            catch (Exception exception)
            {
                _logContext.LogIndexerSetException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterIndexerSet(mockInfo);
        }
    }
}
