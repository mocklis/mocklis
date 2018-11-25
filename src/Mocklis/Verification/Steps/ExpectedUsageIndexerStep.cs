// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsageIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Steps
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Threading;
    using Mocklis.Core;

    #endregion

    public sealed class ExpectedUsageIndexerStep<TKey, TValue> : MedialIndexerStep<TKey, TValue>, IVerifiable
    {
        public string Name { get; }
        private readonly int? _expectedNumberOfGets;
        private int _currentNumberOfGets;
        private readonly int? _expectedNumberOfSets;
        private int _currentNumberOfSets;

        public ExpectedUsageIndexerStep(string name, int? expectedNumberOfGets,
            int? expectedNumberOfSets)
        {
            Name = name;
            _expectedNumberOfGets = expectedNumberOfGets;
            _expectedNumberOfSets = expectedNumberOfSets;
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            Interlocked.Increment(ref _currentNumberOfGets);
            return base.Get(mockInfo, key);
        }

        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            Interlocked.Increment(ref _currentNumberOfSets);
            base.Set(mockInfo, key, value);
        }

        public IEnumerable<VerificationResult> Verify()
        {
            string prefix = string.IsNullOrEmpty(Name) ? "Usage Count" : $"Usage Count '{Name}'";

            if (_expectedNumberOfGets is int expectedGets)
            {
                yield return new VerificationResult($"{prefix}: Expected {expectedGets} get(s); received {_currentNumberOfGets} get(s).",
                    expectedGets == _currentNumberOfGets);
            }

            if (_expectedNumberOfSets is int expectedSets)
            {
                yield return new VerificationResult($"{prefix}: Expected {expectedSets} set(s); received {_currentNumberOfSets} set(s).",
                    expectedSets == _currentNumberOfSets);
            }
        }
    }
}
