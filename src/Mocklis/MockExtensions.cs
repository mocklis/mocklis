// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public static class MockExtensions
    {
        public static MissingEventStep<THandler> UseMissing<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            return caller.SetNextStep(MissingEventStep<THandler>.Instance);
        }

        public static FieldBackedEventStep<THandler> UseBackingField<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            return caller.SetNextStep(new FieldBackedEventStep<THandler>());
        }

        public static FieldBackedEventStep<THandler> UseBackingField<THandler>(
            this IEventStepCaller<THandler> caller,
            out FieldBackedEventStep<THandler> step) where THandler : Delegate
        {
            step = new FieldBackedEventStep<THandler>();
            return caller.SetNextStep(step);
        }

        public static MissingMethodStep<TParam, TResult> UseMissing<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller)
        {
            return caller.SetNextStep(MissingMethodStep<TParam, TResult>.Instance);
        }

        public static ActionMethodStep<TParam> UseAction<TParam>(
            this IMethodStepCaller<TParam, ValueTuple> caller,
            Action<TParam> action)
        {
            return caller.SetNextStep(new ActionMethodStep<TParam>(action));
        }

        public static ActionMethodStep UseAction(
            this IMethodStepCaller<ValueTuple, ValueTuple> caller,
            Action action)
        {
            return caller.SetNextStep(new ActionMethodStep(action));
        }

        public static FuncMethodStep<TParam, TResult> UseFunc<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            Func<TParam, TResult> func)
        {
            return caller.SetNextStep(new FuncMethodStep<TParam, TResult>(func));
        }

        public static FuncMethodStep<TResult> UseFunc<TResult>(
            this IMethodStepCaller<ValueTuple, TResult> caller,
            Func<TResult> func)
        {
            return caller.SetNextStep(new FuncMethodStep<TResult>(func));
        }

        public static MissingPropertyStep<TValue> UseMissing<TValue>(
            this IPropertyStepCaller<TValue> caller)
        {
            return caller.SetNextStep(MissingPropertyStep<TValue>.Instance);
        }

        public static FieldBackedPropertyStep<TValue> UseBackingField<TValue>(
            this IPropertyStepCaller<TValue> caller,
            TValue initialValue = default)
        {
            return caller.SetNextStep(
                new FieldBackedPropertyStep<TValue>(initialValue));
        }

        public static FieldBackedPropertyStep<TValue> UseBackingField<TValue>(
            this IPropertyStepCaller<TValue> caller,
            out FieldBackedPropertyStep<TValue> step,
            TValue initialValue = default)
        {
            step = new FieldBackedPropertyStep<TValue>(initialValue);
            return caller.SetNextStep(step);
        }

        public static MissingIndexerStep<TKey, TValue> UseMissing<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            return caller.SetNextStep(MissingIndexerStep<TKey, TValue>.Instance);
        }

        public static DictionaryBackedIndexerStep<TKey, TValue> UseBackingDictionary<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            return caller.SetNextStep(new DictionaryBackedIndexerStep<TKey, TValue>());
        }

        public static DictionaryBackedIndexerStep<TKey, TValue> UseBackingDictionary<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out DictionaryBackedIndexerStep<TKey, TValue> step)
        {
            step = new DictionaryBackedIndexerStep<TKey, TValue>();
            return caller.SetNextStep(step);
        }

        public static LoggingMethodStep<TParam, TResult> WithLogging<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller)
        {
            return caller.SetNextStep(new LoggingMethodStep<TParam, TResult>());
        }
    }
}
