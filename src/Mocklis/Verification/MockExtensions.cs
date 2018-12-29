// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Verification.Checks;
    using Mocklis.Verification.Steps;

    #endregion

    public static class MockExtensions
    {
        public static ICanHaveNextEventStep<THandler> ExpectedUsage<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            VerificationGroup collector,
            string name,
            int? expectedNumberOfAdds = null,
            int? expectedNumberOfRemoves = null) where THandler : Delegate
        {
            var step = new ExpectedUsageEventStep<THandler>(name, expectedNumberOfAdds, expectedNumberOfRemoves);
            collector.Add(step);
            return caller.SetNextStep(step);
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> ExpectedUsage<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            VerificationGroup collector,
            string name,
            int? expectedNumberOfGets = null,
            int? expectedNumberOfSets = null)
        {
            var step = new ExpectedUsageIndexerStep<TKey, TValue>(name, expectedNumberOfGets, expectedNumberOfSets);
            collector.Add(step);
            return caller.SetNextStep(step);
        }

        public static ICanHaveNextMethodStep<TParam, TResult> ExpectedUsage<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            VerificationGroup collector,
            string name,
            int expectedNumberOfCalls)
        {
            var step = new ExpectedUsageMethodStep<TParam, TResult>(name, expectedNumberOfCalls);
            collector.Add(step);
            return caller.SetNextStep(step);
        }

        public static ICanHaveNextPropertyStep<TValue> ExpectedUsage<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            VerificationGroup collector,
            string name,
            int? expectedNumberOfGets = null,
            int? expectedNumberOfSets = null)
        {
            var step = new ExpectedUsagePropertyStep<TValue>(name, expectedNumberOfGets, expectedNumberOfSets);
            collector.Add(step);
            return caller.SetNextStep(step);
        }

        public static IStoredProperty<TValue> CurrentValueCheck<TValue>(this IStoredProperty<TValue> property, VerificationGroup collector,
            string name, TValue expectedValue, IEqualityComparer<TValue> comparer = null)
        {
            collector.Add(new CurrentValuePropertyCheck<TValue>(property, name, expectedValue, comparer));
            return property;
        }

        public static IStoredIndexer<TKey, TValue> CurrentValuesCheck<TKey, TValue>(this IStoredIndexer<TKey, TValue> indexer,
            VerificationGroup collector,
            string name, IEnumerable<KeyValuePair<TKey, TValue>> expectedValues, IEqualityComparer<TValue> comparer = null)
        {
            collector.Add(new CurrentValuesIndexerCheck<TKey, TValue>(indexer, name, expectedValues, comparer));
            return indexer;
        }
    }
}
