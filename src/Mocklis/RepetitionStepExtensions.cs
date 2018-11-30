// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepetitionStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Times;

    #endregion

    public static class RepetitionStepExtensions
    {
        public static IEventStepCaller<THandler> Times<THandler>(
            this IEventStepCaller<THandler> caller,
            int times,
            Action<IEventStepCaller<THandler>> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new TimesEventStep<THandler>(times, branch));
        }

        public static IIndexerStepCaller<TKey, TValue> Times<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            int times,
            Action<IIndexerStepCaller<TKey, TValue>> branch)
        {
            return caller.SetNextStep(new TimesIndexerStep<TKey, TValue>(times, branch));
        }

        public static IMethodStepCaller<TParam, TResult> Times<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            int times,
            Action<IMethodStepCaller<TParam, TResult>> branch)
        {
            return caller.SetNextStep(new TimesMethodStep<TParam, TResult>(times, branch));
        }

        public static IPropertyStepCaller<TValue> Times<TValue>(
            this IPropertyStepCaller<TValue> caller,
            int times,
            Action<IPropertyStepCaller<TValue>> branch)
        {
            return caller.SetNextStep(new TimesPropertyStep<TValue>(times, branch));
        }
    }
}
