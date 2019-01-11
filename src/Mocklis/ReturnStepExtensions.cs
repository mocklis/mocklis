// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnStepExtensions.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
        public static ICanHaveNextIndexerStep<TKey, TValue> Return<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnIndexerStep<TKey, TValue>(value));
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> ReturnOnce<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnOnceIndexerStep<TKey, TValue>(value));
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> ReturnEach<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            IEnumerable<TValue> values)
        {
            return caller.SetNextStep(new ReturnEachIndexerStep<TKey, TValue>(values));
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> ReturnEach<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            params TValue[] values)
        {
            return caller.ReturnEach(values.AsEnumerable());
        }

        public static void Return<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            TResult result)
        {
            caller.SetNextStep(new ReturnMethodStep<TParam, TResult>(result));
        }

        public static ICanHaveNextMethodStep<TParam, TResult> ReturnOnce<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            TResult result)
        {
            return caller.SetNextStep(new ReturnOnceMethodStep<TParam, TResult>(result));
        }

        public static ICanHaveNextMethodStep<TParam, TResult> ReturnEach<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            IEnumerable<TResult> results)
        {
            return caller.SetNextStep(new ReturnEachMethodStep<TParam, TResult>(results));
        }

        public static ICanHaveNextMethodStep<TParam, TResult> ReturnEach<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            params TResult[] results)
        {
            return caller.ReturnEach(results.AsEnumerable());
        }

        public static ICanHaveNextPropertyStep<TValue> Return<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnPropertyStep<TValue>(value));
        }

        public static ICanHaveNextPropertyStep<TValue> ReturnOnce<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnOncePropertyStep<TValue>(value));
        }

        public static ICanHaveNextPropertyStep<TValue> ReturnEach<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            IEnumerable<TValue> values)
        {
            return caller.SetNextStep(new ReturnEachPropertyStep<TValue>(values));
        }

        public static ICanHaveNextPropertyStep<TValue> ReturnEach<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            params TValue[] values)
        {
            return caller.ReturnEach(values.AsEnumerable());
        }
    }
}
