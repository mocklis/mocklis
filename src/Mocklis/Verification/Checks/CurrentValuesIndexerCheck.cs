// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentValuesIndexerCheck.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Checks
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static System.FormattableString;

    #endregion

    /// <summary>
    ///     Check that verifies that the indexer store has the expected values.
    ///     Implements the <see cref="Mocklis.Verification.IVerifiable" /> interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="Mocklis.Verification.IVerifiable" />
    public class CurrentValuesIndexerCheck<TKey, TValue> : IVerifiable
    {
        private readonly IStoredIndexer<TKey, TValue> _indexer;
        private readonly string _name;
        private readonly IEnumerable<KeyValuePair<TKey, TValue>> _expectations;
        private readonly IEqualityComparer<TValue> _comparer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CurrentValuesIndexerCheck{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="indexer">The indexer store to check.</param>
        /// <param name="name">A name that can be used to identify the check in its verification group.</param>
        /// <param name="expectations">
        ///     A list of key-value pairs to check. The check will retrieve the value for each key
        ///     in the list and compare it to the value in the list.
        /// </param>
        /// <param name="comparer">Optional parameter with a comparer used to verify that the values are equal.</param>
        public CurrentValuesIndexerCheck(IStoredIndexer<TKey, TValue> indexer, string name, IEnumerable<KeyValuePair<TKey, TValue>> expectations,
            IEqualityComparer<TValue> comparer = null)
        {
            _indexer = indexer ?? throw new ArgumentNullException(nameof(indexer));
            _name = name;
            _expectations = expectations ?? Enumerable.Empty<KeyValuePair<TKey, TValue>>();
            _comparer = comparer ?? EqualityComparer<TValue>.Default;
        }

        /// <summary>
        ///     Verifies a set of conditions and returns the result of the verifications. Each key checked in the indexer is
        ///     treated as one such condition.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable{VerificationResult}" /> with information about the verifications and whether they
        ///     were successful.
        /// </returns>
        public IEnumerable<VerificationResult> Verify()
        {
            VerificationResult SubResult(KeyValuePair<TKey, TValue> expectation)
            {
                TKey key = expectation.Key;
                TValue expectedValue = expectation.Value;
                TValue currentValue = _indexer[key];
                string description = Invariant($"Key '{key}'; Expected '{expectedValue}'; Current Value is '{currentValue}'");
                bool success = _comparer.Equals(expectedValue, currentValue);
                return new VerificationResult(description, success);
            }

            string commonDescription = string.IsNullOrEmpty(_name) ? "Values check:" : Invariant($"Values check '{_name}':");

            yield return new VerificationResult(commonDescription, _expectations.Select(SubResult));
        }
    }
}
