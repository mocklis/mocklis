// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JoinStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public static class JoinStepExtensions
    {
        public static void Join<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            IEventStep<THandler> joinPoint) where THandler : Delegate
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            IIndexerStep<TKey, TValue> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            IMethodStep<TParam, TResult> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            IPropertyStep<TValue> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static ICanHaveNextEventStep<THandler> JoinPoint<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            out IEventStep<THandler> joinPoint)
            where THandler : Delegate
        {
            var joinStep = new EventStepWithNext<THandler>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> JoinPoint<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IIndexerStep<TKey, TValue> joinPoint)
        {
            var joinStep = new IndexerStepWithNext<TKey, TValue>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static ICanHaveNextMethodStep<TParam, TResult> JoinPoint<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IMethodStep<TParam, TResult> joinPoint)
        {
            var joinStep = new MethodStepWithNext<TParam, TResult>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static ICanHaveNextPropertyStep<TValue> JoinPoint<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IPropertyStep<TValue> joinPoint)
        {
            var joinStep = new PropertyStepWithNext<TValue>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }
    }
}
