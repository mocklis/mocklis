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
    using Mocklis.Steps.Throw;

    #endregion

    public static class ThrowStepExtensions
    {
        public static void Throw<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Func<Exception> exceptionFactory) where THandler : Delegate
        {
            caller.SetNextStep(new ThrowEventStep<THandler>(exceptionFactory));
        }

        public static void Throw<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<TKey, Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowIndexerStep<TKey, TValue>(exceptionFactory));
        }

        public static void Throw<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowMethodStep<TResult>(exceptionFactory));
        }

        public static void Throw<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<TParam, Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowMethodStep<TParam, TResult>(exceptionFactory));
        }

        public static void Throw<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowPropertyStep<TValue>(exceptionFactory));
        }
    }
}
