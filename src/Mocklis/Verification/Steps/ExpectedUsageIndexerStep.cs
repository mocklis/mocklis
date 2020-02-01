// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsageIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Steps
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Indexer step that counts the number of times values have been read from and written to the indexer, and can verify
    ///     these against given expected number of reads and writes. This class cannot be inherited.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    ///     Implements the <see cref="IVerifiable" /> interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    /// <seealso cref="IVerifiable" />
    public sealed class ExpectedUsageIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>, IVerifiable
    {
        private readonly string? _name;
        private readonly int? _expectedNumberOfGets;
        private int _currentNumberOfGets;
        private readonly int? _expectedNumberOfSets;
        private int _currentNumberOfSets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpectedUsageIndexerStep{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="name">The name of the verification.</param>
        /// <param name="expectedNumberOfGets">The expected number of reads from the indexer.</param>
        /// <param name="expectedNumberOfSets">The expected number of writes to the indexer.</param>
        public ExpectedUsageIndexerStep(string? name, int? expectedNumberOfGets,
            int? expectedNumberOfSets)
        {
            if (expectedNumberOfGets < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedNumberOfGets),
                    "Expected number of gets must not be negative. Pass 'null' to remove check.");
            }

            if (expectedNumberOfSets < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedNumberOfSets),
                    "Expected number of sets must not be negative. Pass 'null' to remove check.");
            }

            _name = name;
            _expectedNumberOfGets = expectedNumberOfGets;
            _expectedNumberOfSets = expectedNumberOfSets;
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     Increases a counter that keeps track of the number of times values have been read.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            Interlocked.Increment(ref _currentNumberOfGets);
            return base.Get(mockInfo, key);
        }

        /// <summary>
        ///     Called when a value is written to the indexer.
        ///     Increases a counter that keeps track of the number of times values have been written.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            Interlocked.Increment(ref _currentNumberOfSets);
            base.Set(mockInfo, key, value);
        }

        /// <summary>
        ///     Verifies that the expected number of reads from and writes to the indexer have occurred and returns the result of
        ///     the verifications.
        /// </summary>
        /// <param name="provider">
        ///     An object that supplies culture-specific formatting information. Not used for this implementation since
        ///     we'd only be formatting non-negative <see cref="int" /> values.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable{VerificationResult}" /> with information about the verifications and whether they
        ///     were successful.
        /// </returns>
        public IEnumerable<VerificationResult> Verify(IFormatProvider? provider = null)
        {
            string prefix = string.IsNullOrEmpty(_name) ? "Usage Count" : $"Usage Count '{_name}'";

            if (_expectedNumberOfGets is int expectedGets)
            {
                string expectedGetsString = expectedGets.ToString();
                string currentGetsString = _currentNumberOfGets.ToString();
                yield return new VerificationResult($"{prefix}: Expected {expectedGetsString} get(s); received {currentGetsString} get(s).",
                    expectedGets == _currentNumberOfGets);
            }

            if (_expectedNumberOfSets is int expectedSets)
            {
                string expectedSetsString = expectedSets.ToString();
                string currentSetsString = _currentNumberOfSets.ToString();

                yield return new VerificationResult($"{prefix}: Expected {expectedSetsString} set(s); received {currentSetsString} set(s).",
                    expectedSets == _currentNumberOfSets);
            }
        }
    }
}
