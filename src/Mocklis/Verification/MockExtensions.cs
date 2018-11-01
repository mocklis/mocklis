// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public static class MockExtensions
    {
        public static UsageCountingMethodStep<TParam, TResult> WithUsageCounting<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            VerificationGroup collector,
            string name,
            int expectedNumberOfCalls)
        {
            var step = new UsageCountingMethodStep<TParam, TResult>(name, expectedNumberOfCalls);
            collector.Add(step);
            return caller.SetNextStep(step);
        }

        public static UsageCountingPropertyStep<TValue> WithUsageCounting<TValue>(
            this IPropertyStepCaller<TValue> caller,
            VerificationGroup collector,
            string name,
            int? expectedNumberOfGets = null,
            int? expectedNumberOfSets = null)
        {
            var step = new UsageCountingPropertyStep<TValue>(name, expectedNumberOfGets, expectedNumberOfSets);
            collector.Add(step);
            return caller.SetNextStep(step);
        }
    }
}
