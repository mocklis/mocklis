// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsageEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

    public sealed class ExpectedUsageEventStep<THandler> : EventStepWithNext<THandler>, IVerifiable where THandler : Delegate
    {
        public string Name { get; }
        private readonly int? _expectedNumberOfAdds;
        private int _currentNumberOfAdds;
        private readonly int? _expectedNumberOfRemoves;
        private int _currentNumberOfRemoves;

        public ExpectedUsageEventStep(string name, int? expectedNumberOfAdds,
            int? expectedNumberOfRemoves)
        {
            _expectedNumberOfAdds = expectedNumberOfAdds;
            _expectedNumberOfRemoves = expectedNumberOfRemoves;
            Name = name;
        }

        public override void Add(IMockInfo mockInfo, THandler value)
        {
            Interlocked.Increment(ref _currentNumberOfAdds);
            base.Add(mockInfo, value);
        }

        public override void Remove(IMockInfo mockInfo, THandler value)
        {
            Interlocked.Increment(ref _currentNumberOfRemoves);
            base.Remove(mockInfo, value);
        }

        public IEnumerable<VerificationResult> Verify()
        {
            string prefix = string.IsNullOrEmpty(Name) ? "Usage Count" : $"Usage Count '{Name}'";

            if (_expectedNumberOfAdds is int expectedAdds)
            {
                yield return new VerificationResult($"{prefix}: Expected {expectedAdds} add(s); received {_currentNumberOfAdds} add(s).",
                    expectedAdds == _currentNumberOfAdds);
            }

            if (_expectedNumberOfRemoves is int expectedRemoves)
            {
                yield return new VerificationResult($"{prefix}: Expected {expectedRemoves} remove(s); received {_currentNumberOfRemoves} remove(s).",
                    expectedRemoves == _currentNumberOfRemoves);
            }
        }
    }
}
