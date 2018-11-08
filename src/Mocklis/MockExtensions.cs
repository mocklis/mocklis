// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Mocklis.Conditional;
    using Mocklis.Core;
    using Mocklis.Dummy;
    using Mocklis.Lambda;
    using Mocklis.Log;
    using Mocklis.Miscellaneous;
    using Mocklis.Return;
    using Mocklis.Stored;

    #endregion

    public static class MockExtensions
    {
        #region 'Conditional' steps

        public static IIndexerStepCaller<TKey, TValue> If<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, Func<TKey, bool> condition,
            Action<IIndexerStepCaller<TKey, TValue>> branch)
        {
            return caller.SetNextStep(new IfIndexerStep<TKey, TValue>(condition, branch));
        }

        public static IMethodStepCaller<TParam, TResult> If<TParam, TResult>(this IMethodStepCaller<TParam, TResult> caller,
            Func<TParam, bool> condition, Action<IMethodStepCaller<TParam, TResult>> branch)
        {
            return caller.SetNextStep(new IfMethodStep<TParam, TResult>(condition, branch));
        }

        #endregion

        #region 'Dummy' steps

        public static IFinalStep Dummy<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            return caller.SetNextStep(DummyEventStep<THandler>.Instance);
        }

        public static IFinalStep Dummy<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            return caller.SetNextStep(DummyIndexerStep<TKey, TValue>.Instance);
        }

        public static IFinalStep Dummy<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller)
        {
            return caller.SetNextStep(DummyMethodStep<TParam, TResult>.Instance);
        }

        public static IFinalStep Dummy<TValue>(
            this IPropertyStepCaller<TValue> caller)
        {
            return caller.SetNextStep(DummyPropertyStep<TValue>.Instance);
        }

        #endregion

        #region 'Lambda' steps

        public static IFinalStep Func<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, Func<TKey, TValue> func)
        {
            return caller.SetNextStep(new FuncIndexerStep<TKey, TValue>(func));
        }

        public static IFinalStep Action<TParam>(
            this IMethodStepCaller<TParam, ValueTuple> caller,
            Action<TParam> action)
        {
            return caller.SetNextStep(new ActionMethodStep<TParam>(action));
        }

        public static IFinalStep Action(
            this IMethodStepCaller<ValueTuple, ValueTuple> caller,
            Action action)
        {
            return caller.SetNextStep(new ActionMethodStep(action));
        }

        public static IFinalStep Func<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            Func<TParam, TResult> func)
        {
            return caller.SetNextStep(new FuncMethodStep<TParam, TResult>(func));
        }

        public static IFinalStep Func<TResult>(
            this IMethodStepCaller<ValueTuple, TResult> caller,
            Func<TResult> func)
        {
            return caller.SetNextStep(new FuncMethodStep<TResult>(func));
        }

        #endregion

        #region 'Log' Steps

        public static IEventStepCaller<THandler> Log<THandler>(this IEventStepCaller<THandler> caller, ILogContext logContext)
            where THandler : Delegate
        {
            return caller.SetNextStep(new LogEventStep<THandler>(logContext ?? TextWriterLogContext.Console));
        }

        public static IIndexerStepCaller<TKey, TValue> Log<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, ILogContext logContext)
        {
            return caller.SetNextStep(new LogIndexerStep<TKey, TValue>(logContext ?? TextWriterLogContext.Console));
        }

        public static IMethodStepCaller<TParam, TResult> Log<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller, ILogContext logContext)
        {
            return caller.SetNextStep(new LogMethodStep<TParam, TResult>(logContext ?? TextWriterLogContext.Console));
        }

        public static IPropertyStepCaller<TValue> Log<TValue>(this IPropertyStepCaller<TValue> caller, ILogContext logContext)
        {
            return caller.SetNextStep(new LogPropertyStep<TValue>(logContext ?? TextWriterLogContext.Console));
        }

        #endregion

        #region 'Miscellaneous' steps

        public static IPropertyStepCaller<TValue> OnlySetIfChanged<TValue>(this IPropertyStepCaller<TValue> caller,
            IEqualityComparer<TValue> comparer = null)
        {
            return caller.SetNextStep(new OnlySetIfChangedPropertyStep<TValue>(comparer));
        }

        public static IIndexerStepCaller<TKey, TValue> OnlySetIfChanged<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller,
            IEqualityComparer<TValue> comparer = null)
        {
            return caller.SetNextStep(new OnlySetIfChangedIndexerStep<TKey, TValue>(comparer));
        }

        public static IPropertyStepCaller<TValue> RaisePropertyChangedEvent<TValue>(
            this IPropertyStepCaller<TValue> caller,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent)
        {
            return caller.SetNextStep(new RaisePropertyChangedEventPropertyStep<TValue>(propertyChangedEvent));
        }

        public static IFinalStep StoredWithChangeNotification<TValue>(
            this IPropertyStepCaller<TValue> caller,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent,
            TValue initialValue = default,
            IEqualityComparer<TValue> comparer = null)
        {
            return caller
                .OnlySetIfChanged(comparer)
                .RaisePropertyChangedEvent(propertyChangedEvent)
                .Stored(initialValue);
        }

        public static IFinalStep StoredWithChangeNotification<TValue>(
            this IPropertyStepCaller<TValue> caller,
            out StoredPropertyStep<TValue> storedPropertyStep,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent,
            TValue initialValue = default,
            IEqualityComparer<TValue> comparer = null)
        {
            return caller
                .OnlySetIfChanged(comparer)
                .RaisePropertyChangedEvent(propertyChangedEvent)
                .Stored(out storedPropertyStep, initialValue);
        }

        #endregion

        #region 'Missing' steps

        public static IFinalStep Missing<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            return caller.SetNextStep(MissingEventStep<THandler>.Instance);
        }

        public static IFinalStep Missing<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            return caller.SetNextStep(MissingIndexerStep<TKey, TValue>.Instance);
        }

        public static IFinalStep Missing<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller)
        {
            return caller.SetNextStep(MissingMethodStep<TParam, TResult>.Instance);
        }

        public static IFinalStep Missing<TValue>(
            this IPropertyStepCaller<TValue> caller)
        {
            return caller.SetNextStep(MissingPropertyStep<TValue>.Instance);
        }

        #endregion

        #region 'Return' steps

        public static IFinalStep Return<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, TValue value)
        {
            return caller.SetNextStep(new ReturnIndexerStep<TKey, TValue>(value));
        }

        public static IIndexerStepCaller<TKey, TValue> ReturnOnce<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, TValue value)
        {
            return caller.SetNextStep(new ReturnOnceIndexerStep<TKey, TValue>(value));
        }

        public static IFinalStep Return<TParam, TResult>(this IMethodStepCaller<TParam, TResult> caller, TResult result)
        {
            return caller.SetNextStep(new ReturnMethodStep<TParam, TResult>(result));
        }

        public static IMethodStepCaller<TParam, TResult> ReturnOnce<TParam, TResult>(this IMethodStepCaller<TParam, TResult> caller, TResult result)
        {
            return caller.SetNextStep(new ReturnOnceMethodStep<TParam, TResult>(result));
        }

        public static IFinalStep Return<TValue>(this IPropertyStepCaller<TValue> caller, TValue value)
        {
            return caller.SetNextStep(new ReturnPropertyStep<TValue>(value));
        }

        public static IPropertyStepCaller<TValue> ReturnOnce<TValue>(this IPropertyStepCaller<TValue> caller, TValue value)
        {
            return caller.SetNextStep(new ReturnOncePropertyStep<TValue>(value));
        }

        #endregion

        #region 'Stored' steps

        public static IFinalStep Stored<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            return caller.SetNextStep(new StoredEventStep<THandler>());
        }

        public static IFinalStep Stored<THandler>(
            this IEventStepCaller<THandler> caller,
            out StoredEventStep<THandler> step) where THandler : Delegate
        {
            step = new StoredEventStep<THandler>();
            return caller.SetNextStep(step);
        }

        public static IFinalStep Stored<TValue>(
            this IPropertyStepCaller<TValue> caller,
            TValue initialValue = default)
        {
            return caller.SetNextStep(
                new StoredPropertyStep<TValue>(initialValue));
        }

        public static IFinalStep Stored<TValue>(
            this IPropertyStepCaller<TValue> caller,
            out StoredPropertyStep<TValue> step,
            TValue initialValue = default)
        {
            step = new StoredPropertyStep<TValue>(initialValue);
            return caller.SetNextStep(step);
        }

        public static IFinalStep StoredAsDictionary<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            return caller.SetNextStep(new StoredAsDictionaryIndexerStep<TKey, TValue>());
        }

        public static IFinalStep StoredAsDictionary<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out StoredAsDictionaryIndexerStep<TKey, TValue> step)
        {
            step = new StoredAsDictionaryIndexerStep<TKey, TValue>();
            return caller.SetNextStep(step);
        }

        #endregion
    }
}
