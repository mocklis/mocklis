// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsageCountingMethodStep.cs">
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

    public sealed class UsageCountingMethodStep<TParam, TResult> : MethodStepCaller<TParam, TResult>, IMethodStep<TParam, TResult>, IVerifiable
    {
        public string Name { get; }
        private readonly int _expectedNumberOfCalls;
        private int _currentNumberOfCalls;

        public UsageCountingMethodStep(string name, int expectedNumberOfCalls)
        {
            Name = name;
            _expectedNumberOfCalls = expectedNumberOfCalls;
        }

        public TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            Interlocked.Increment(ref _currentNumberOfCalls);
            return NextStep.Call(instance, memberMock, param);
        }

        public IEnumerable<VerificationResult> Verify()
        {
            string prefix = string.IsNullOrEmpty(Name) ? "Usage Count" : $"Usage Count '{Name}'";
            yield return new VerificationResult($"{prefix}: Expected {_expectedNumberOfCalls} call(s); received {_currentNumberOfCalls} call(s).",
                _expectedNumberOfCalls == _currentNumberOfCalls);
        }
    }
}
