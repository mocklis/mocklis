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
        public static ICanHaveNextIndexerStep<TKey, TValue> Func<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<TKey, TValue> func)
        {
            return caller.SetNextStep(new FuncIndexerStep<TKey, TValue>(func));
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceFunc<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<object, TKey, TValue> func)
        {
            return caller.SetNextStep(new InstanceFuncIndexerStep<TKey, TValue>(func));
        }

        public static void Action<TParam>(
            this ICanHaveNextMethodStep<TParam, ValueTuple> caller,
            Action<TParam> action)
        {
            caller.SetNextStep(new ActionMethodStep<TParam>(action));
        }

        public static void Action(
            this ICanHaveNextMethodStep<ValueTuple, ValueTuple> caller,
            Action action)
        {
            caller.SetNextStep(new ActionMethodStep(action));
        }

        public static void Func<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<TParam, TResult> func)
        {
            caller.SetNextStep(new FuncMethodStep<TParam, TResult>(func));
        }

        public static void Func<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<TResult> func)
        {
            caller.SetNextStep(new FuncMethodStep<TResult>(func));
        }

        public static void InstanceAction<TParam>(
            this ICanHaveNextMethodStep<TParam, ValueTuple> caller,
            Action<object, TParam> action)
        {
            caller.SetNextStep(new InstanceActionMethodStep<TParam>(action));
        }

        public static void InstanceAction(
            this ICanHaveNextMethodStep<ValueTuple, ValueTuple> caller,
            Action<object> action)
        {
            caller.SetNextStep(new InstanceActionMethodStep(action));
        }

        public static void InstanceFunc<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<object, TParam, TResult> func)
        {
            caller.SetNextStep(new InstanceFuncMethodStep<TParam, TResult>(func));
        }

        public static void InstanceFunc<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<object, TResult> func)
        {
            caller.SetNextStep(new InstanceFuncMethodStep<TResult>(func));
        }

        public static ICanHaveNextPropertyStep<TValue> Func<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<TValue> func)
        {
            return caller.SetNextStep(new FuncPropertyStep<TValue>(func));
        }

        public static ICanHaveNextPropertyStep<TValue> InstanceFunc<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<object, TValue> func)
        {
            return caller.SetNextStep(new InstanceFuncPropertyStep<TValue>(func));
        }
    }
}
