// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Throw;

    #endregion

    public static class ThrowStepExtensions
    {
        public static void Throw<THandler>(
            this IEventStepCaller<THandler> caller,
            Func<Exception> exceptionFactory) where THandler : Delegate
        {
            caller.SetNextStep(new ThrowEventStep<THandler>(exceptionFactory));
        }

        public static IEventStepCaller<THandler> ThrowOnAdd<THandler>(
            this IEventStepCaller<THandler> caller,
            Func<Exception> exceptionFactory)
            where THandler : Delegate
        {
            return caller.SetNextStep(new ThrowOnAddEventStep<THandler>(exceptionFactory));
        }

        public static IEventStepCaller<THandler> ThrowOnRemove<THandler>(
            this IEventStepCaller<THandler> caller,
            Func<Exception> exceptionFactory)
            where THandler : Delegate
        {
            return caller.SetNextStep(new ThrowOnRemoveEventStep<THandler>(exceptionFactory));
        }

        public static void Throw<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Func<TKey, Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowIndexerStep<TKey, TValue>(exceptionFactory));
        }

        public static IIndexerStepCaller<TKey, TValue> ThrowOnGet<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Func<TKey, Exception> exceptionFactory)
        {
            return caller.SetNextStep(new ThrowOnGetIndexerStep<TKey, TValue>(exceptionFactory));
        }

        public static IIndexerStepCaller<TKey, TValue> ThrowOnSet<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Func<TKey, Exception> exceptionFactory)
        {
            return caller.SetNextStep(new ThrowOnSetIndexerStep<TKey, TValue>(exceptionFactory));
        }

        public static void Throw<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            Func<TParam, Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowMethodStep<TParam, TResult>(exceptionFactory));
        }

        public static void Throw<TValue>(
            this IPropertyStepCaller<TValue> caller,
            Func<Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowPropertyStep<TValue>(exceptionFactory));
        }

        public static IPropertyStepCaller<TValue> ThrowOnGet<TValue>(
            this IPropertyStepCaller<TValue> caller,
            Func<Exception> exceptionFactory)
        {
            return caller.SetNextStep(new ThrowOnGetPropertyStep<TValue>(exceptionFactory));
        }

        public static IPropertyStepCaller<TValue> ThrowOnSet<TValue>(
            this IPropertyStepCaller<TValue> caller,
            Func<Exception> exceptionFactory)
        {
            return caller.SetNextStep(new ThrowOnSetPropertyStep<TValue>(exceptionFactory));
        }
    }
}
