// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsageMethodStep.cs">
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

    public sealed class ExpectedUsageMethodStep<TParam, TResult> : MedialMethodStep<TParam, TResult>, IVerifiable
    {
        public string Name { get; }
        private readonly int _expectedNumberOfCalls;
        private int _currentNumberOfCalls;

        public ExpectedUsageMethodStep(string name, int expectedNumberOfCalls)
        {
            Name = name;
            _expectedNumberOfCalls = expectedNumberOfCalls;
        }

        public override TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            Interlocked.Increment(ref _currentNumberOfCalls);
            return base.Call(instance, memberMock, param);
        }

        public IEnumerable<VerificationResult> Verify()
        {
            string prefix = string.IsNullOrEmpty(Name) ? "Usage Count" : $"Usage Count '{Name}'";
            yield return new VerificationResult($"{prefix}: Expected {_expectedNumberOfCalls} call(s); received {_currentNumberOfCalls} call(s).",
                _expectedNumberOfCalls == _currentNumberOfCalls);
        }
    }
}
