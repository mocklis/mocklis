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
            this IEventStepCaller<THandler> caller,
            IEventStep<THandler> joinPoint) where THandler : Delegate
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            IIndexerStep<TKey, TValue> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            IMethodStep<TParam, TResult> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TValue>(
            this IPropertyStepCaller<TValue> caller,
            IPropertyStep<TValue> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static IEventStepCaller<THandler> JoinPoint<THandler>(
            this IEventStepCaller<THandler> caller,
            out IEventStep<THandler> joinPoint)
            where THandler : Delegate
        {
            var joinStep = new MedialEventStep<THandler>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static IIndexerStepCaller<TKey, TValue> JoinPoint<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out IIndexerStep<TKey, TValue> joinPoint)
        {
            var joinStep = new MedialIndexerStep<TKey, TValue>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static IMethodStepCaller<TParam, TResult> JoinPoint<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            out IMethodStep<TParam, TResult> joinPoint)
        {
            var joinStep = new MedialMethodStep<TParam, TResult>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static IPropertyStepCaller<TValue> JoinPoint<TValue>(
            this IPropertyStepCaller<TValue> caller,
            out IPropertyStep<TValue> joinPoint)
        {
            var joinStep = new MedialPropertyStep<TValue>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }
    }
}
