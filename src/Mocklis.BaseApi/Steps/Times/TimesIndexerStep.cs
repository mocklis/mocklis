// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Repetition' indexer step that takes an alternative branch a given number of times, and the
    ///     normal branch thereafter.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public class TimesIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly object _lockObject = new object();
        private readonly int _times;
        private int _calls;
        private readonly IndexerStepWithNext<TKey, TValue> _branch = new IndexerStepWithNext<TKey, TValue>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimesIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="times">The number of times the alternative branch should be taken.</param>
        /// <param name="branch">An action to set up the alternative branch.</param>
        public TimesIndexerStep(int times, Action<ICanHaveNextIndexerStep<TKey, TValue>> branch)
        {
            _times = times;
            branch.Invoke(_branch);
        }

        private bool ShouldUseBranch()
        {
            lock (_lockObject)
            {
                if (_calls < _times)
                {
                    _calls++;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This will chose the alternative branch for a given number of reads or writes (counted together) and the normal
        ///     branch afterwards.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return ShouldUseBranch() ? _branch.Get(mockInfo, key) : base.Get(mockInfo, key);
        }

        /// <summary>
        ///     Called when a value is written to the indexer.
        ///     This will chose the alternative branch for a given number of reads or writes (counted together) and the normal
        ///     branch afterwards.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            if (ShouldUseBranch())
            {
                _branch.Set(mockInfo, key, value);
            }
            else
            {
                base.Set(mockInfo, key, value);
            }
        }
    }
}
