// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LambdaStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Lambda;

    #endregion

    public static class LambdaStepExtensions
    {
        public static void Func<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Func<TKey, TValue> func)
        {
            caller.SetNextStep(new FuncIndexerStep<TKey, TValue>(func));
        }

        public static void InstanceFunc<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Func<object, TKey, TValue> func)
        {
            caller.SetNextStep(new InstanceFuncIndexerStep<TKey, TValue>(func));
        }

        public static void Action<TParam>(
            this IMethodStepCaller<TParam, ValueTuple> caller,
            Action<TParam> action)
        {
            caller.SetNextStep(new ActionMethodStep<TParam>(action));
        }

        public static void Action(
            this IMethodStepCaller<ValueTuple, ValueTuple> caller,
            Action action)
        {
            caller.SetNextStep(new ActionMethodStep(action));
        }

        public static void Func<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            Func<TParam, TResult> func)
        {
            caller.SetNextStep(new FuncMethodStep<TParam, TResult>(func));
        }

        public static void Func<TResult>(
            this IMethodStepCaller<ValueTuple, TResult> caller,
            Func<TResult> func)
        {
            caller.SetNextStep(new FuncMethodStep<TResult>(func));
        }

        public static void InstanceAction<TParam>(
            this IMethodStepCaller<TParam, ValueTuple> caller,
            Action<object, TParam> action)
        {
            caller.SetNextStep(new InstanceActionMethodStep<TParam>(action));
        }

        public static void InstanceAction(
            this IMethodStepCaller<ValueTuple, ValueTuple> caller,
            Action<object> action)
        {
            caller.SetNextStep(new InstanceActionMethodStep(action));
        }

        public static void InstanceFunc<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            Func<object, TParam, TResult> func)
        {
            caller.SetNextStep(new InstanceFuncMethodStep<TParam, TResult>(func));
        }

        public static void InstanceFunc<TResult>(
            this IMethodStepCaller<ValueTuple, TResult> caller,
            Func<object, TResult> func)
        {
            caller.SetNextStep(new InstanceFuncMethodStep<TResult>(func));
        }

        public static void InstanceFunc<TValue>(
            this IPropertyStepCaller<TValue> caller,
            Func<object, TValue> func)
        {
            caller.SetNextStep(new InstanceFuncPropertyStep<TValue>(func));
        }
    }
}
