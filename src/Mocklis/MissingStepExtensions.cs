// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public static class MissingStepExtensions
    {
        public static void Missing<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            caller.SetNextStep(MissingEventStep<THandler>.Instance);
        }

        public static void Missing<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            caller.SetNextStep(MissingIndexerStep<TKey, TValue>.Instance);
        }

        public static void Missing<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller)
        {
            caller.SetNextStep(MissingMethodStep<TParam, TResult>.Instance);
        }

        public static void Missing<TValue>(
            this IPropertyStepCaller<TValue> caller)
        {
            caller.SetNextStep(MissingPropertyStep<TValue>.Instance);
        }
    }
}
