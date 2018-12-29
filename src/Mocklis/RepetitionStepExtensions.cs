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
        public static ICanHaveNextEventStep<THandler> Times<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            int times,
            Action<ICanHaveNextEventStep<THandler>> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new TimesEventStep<THandler>(times, branch));
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> Times<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            int times,
            Action<ICanHaveNextIndexerStep<TKey, TValue>> branch)
        {
            return caller.SetNextStep(new TimesIndexerStep<TKey, TValue>(times, branch));
        }

        public static ICanHaveNextMethodStep<TParam, TResult> Times<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            int times,
            Action<ICanHaveNextMethodStep<TParam, TResult>> branch)
        {
            return caller.SetNextStep(new TimesMethodStep<TParam, TResult>(times, branch));
        }

        public static ICanHaveNextPropertyStep<TValue> Times<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            int times,
            Action<ICanHaveNextPropertyStep<TValue>> branch)
        {
            return caller.SetNextStep(new TimesPropertyStep<TValue>(times, branch));
        }
    }
}
