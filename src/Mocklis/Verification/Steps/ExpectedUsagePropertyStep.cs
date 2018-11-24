// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsagePropertyStep.cs">
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

    public sealed class ExpectedUsagePropertyStep<TValue> : MedialPropertyStep<TValue>, IVerifiable
    {
        private readonly string _name;
        private readonly int? _expectedNumberOfGets;
        private int _currentNumberOfGets;
        private readonly int? _expectedNumberOfSets;
        private int _currentNumberOfSets;

        public ExpectedUsagePropertyStep(string name, int? expectedNumberOfGets,
            int? expectedNumberOfSets)
        {
            _name = name;
            _expectedNumberOfGets = expectedNumberOfGets;
            _expectedNumberOfSets = expectedNumberOfSets;
        }

        public override TValue Get(MemberMock memberMock)
        {
            Interlocked.Increment(ref _currentNumberOfGets);
            return base.Get(memberMock);
        }

        public override void Set(MemberMock memberMock, TValue value)
        {
            Interlocked.Increment(ref _currentNumberOfSets);
            base.Set(memberMock, value);
        }

        public IEnumerable<VerificationResult> Verify()
        {
            string prefix = string.IsNullOrEmpty(_name) ? "Usage Count" : $"Usage Count '{_name}'";

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
