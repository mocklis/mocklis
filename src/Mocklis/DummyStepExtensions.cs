// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Dummy;

    #endregion

    public static class DummyStepExtensions
    {
        public static void Dummy<THandler>(
            this ICanHaveNextEventStep<THandler> caller) where THandler : Delegate
        {
            caller.SetNextStep(DummyEventStep<THandler>.Instance);
        }

        public static void Dummy<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller)
        {
            caller.SetNextStep(DummyIndexerStep<TKey, TValue>.Instance);
        }

        public static void Dummy<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller)
        {
            caller.SetNextStep(DummyMethodStep<TParam, TResult>.Instance);
        }

        public static void Dummy<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller)
        {
            caller.SetNextStep(DummyPropertyStep<TValue>.Instance);
        }
    }
}
