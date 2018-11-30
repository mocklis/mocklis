// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Mocklis.Core;
    using Mocklis.Steps.Return;

    #endregion

    public static class ReturnStepExtensions
    {
        public static void Return<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            TValue value)
        {
            caller.SetNextStep(new ReturnIndexerStep<TKey, TValue>(value));
        }

        public static IIndexerStepCaller<TKey, TValue> ReturnOnce<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnOnceIndexerStep<TKey, TValue>(value));
        }

        public static IIndexerStepCaller<TKey, TValue> ReturnEach<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            IEnumerable<TValue> values)
        {
            return caller.SetNextStep(new ReturnEachIndexerStep<TKey, TValue>(values));
        }

        public static IIndexerStepCaller<TKey, TValue> ReturnEach<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            params TValue[] values)
        {
            return caller.ReturnEach(values.AsEnumerable());
        }

        public static void Return<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            TResult result)
        {
            caller.SetNextStep(new ReturnMethodStep<TParam, TResult>(result));
        }

        public static IMethodStepCaller<TParam, TResult> ReturnOnce<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            TResult result)
        {
            return caller.SetNextStep(new ReturnOnceMethodStep<TParam, TResult>(result));
        }

        public static IMethodStepCaller<TParam, TResult> ReturnEach<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            IEnumerable<TResult> results)
        {
            return caller.SetNextStep(new ReturnEachMethodStep<TParam, TResult>(results));
        }

        public static IMethodStepCaller<TParam, TResult> ReturnEach<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            params TResult[] results)
        {
            return caller.ReturnEach(results.AsEnumerable());
        }

        public static void Return<TValue>(
            this IPropertyStepCaller<TValue> caller,
            TValue value)
        {
            caller.SetNextStep(new ReturnPropertyStep<TValue>(value));
        }

        public static IPropertyStepCaller<TValue> ReturnOnce<TValue>(
            this IPropertyStepCaller<TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnOncePropertyStep<TValue>(value));
        }

        public static IPropertyStepCaller<TValue> ReturnEach<TValue>(
            this IPropertyStepCaller<TValue> caller,
            IEnumerable<TValue> values)
        {
            return caller.SetNextStep(new ReturnEachPropertyStep<TValue>(values));
        }

        public static IPropertyStepCaller<TValue> ReturnEach<TValue>(
            this IPropertyStepCaller<TValue> caller,
            params TValue[] values)
        {
            return caller.ReturnEach(values.AsEnumerable());
        }
    }
}
