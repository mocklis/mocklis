// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingStepExtensions.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Missing;

    #endregion

    public static class MissingStepExtensions
    {
        public static void Missing<THandler>(
            this ICanHaveNextEventStep<THandler> caller) where THandler : Delegate
        {
            caller.SetNextStep(MissingEventStep<THandler>.Instance);
        }

        public static void Missing<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller)
        {
            caller.SetNextStep(MissingIndexerStep<TKey, TValue>.Instance);
        }

        public static void Missing<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller)
        {
            caller.SetNextStep(MissingMethodStep<TParam, TResult>.Instance);
        }

        public static void Missing<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller)
        {
            caller.SetNextStep(MissingPropertyStep<TValue>.Instance);
        }
    }
}
