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

    public class CurrentValuesIndexerCheck<TKey, TValue> : IVerifiable
    {
        private readonly IStoredIndexer<TKey, TValue> _indexer;
        private readonly string _name;
        private readonly IEnumerable<KeyValuePair<TKey, TValue>> _expectations;
        private readonly IEqualityComparer<TValue> _comparer;

        public CurrentValuesIndexerCheck(IStoredIndexer<TKey, TValue> indexer, string name, IEnumerable<KeyValuePair<TKey, TValue>> expectations,
            IEqualityComparer<TValue> comparer = null)
        {
            _indexer = indexer ?? throw new ArgumentNullException(nameof(indexer));
            _name = name;
            _expectations = expectations ?? Enumerable.Empty<KeyValuePair<TKey, TValue>>();
            _comparer = comparer ?? EqualityComparer<TValue>.Default;
        }

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
