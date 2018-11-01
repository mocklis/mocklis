// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsageCountingPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Threading;
    using Mocklis.Core;
    using Mocklis.StepCallerBaseClasses;

    #endregion

    public sealed class UsageCountingPropertyStep<TValue> : PropertyStepCaller<TValue>, IPropertyStep<TValue>, IVerifiable
    {
        public string Name { get; }
        private readonly int? _expectedNumberOfGets;
        private int _currentNumberOfGets;
        private readonly int? _expectedNumberOfSets;
        private int _currentNumberOfSets;

        public UsageCountingPropertyStep(string name, int? expectedNumberOfGets,
            int? expectedNumberOfSets)
        {
            Name = name;
            _expectedNumberOfGets = expectedNumberOfGets;
            _expectedNumberOfSets = expectedNumberOfSets;
        }

        public TValue Get(MemberMock memberMock)
        {
            Interlocked.Increment(ref _currentNumberOfGets);
            return NextStep.Get(memberMock);
        }

        public void Set(MemberMock memberMock, TValue value)
        {
            Interlocked.Increment(ref _currentNumberOfSets);
            NextStep.Set(memberMock, value);
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
