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
            Func<TParam, bool> condition, Action<IMethodStepCaller<TParam, TResult>, IMethodStep<TParam, TResult>> ifBranch)
        {
            return caller.SetNextStep(new IfMethodStep<TParam, TResult>(condition, ifBranch));
        }

        #endregion

        #region 'Dummy' steps

        public static void Dummy<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            caller.SetNextStep(DummyEventStep<THandler>.Instance);
        }

        public static void Dummy<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            caller.SetNextStep(DummyIndexerStep<TKey, TValue>.Instance);
        }

        public static void Dummy<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller)
        {
            caller.SetNextStep(DummyMethodStep<TParam, TResult>.Instance);
        }

        public static void Dummy<TValue>(
            this IPropertyStepCaller<TValue> caller)
        {
            caller.SetNextStep(DummyPropertyStep<TValue>.Instance);
        }

        #endregion

        #region 'Join' steps

        public static void Join<THandler>(this IEventStepCaller<THandler> caller, IEventStep<THandler> joinPoint) where THandler : Delegate
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, IIndexerStep<TKey, TValue> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TParam, TResult>(this IMethodStepCaller<TParam, TResult> caller, IMethodStep<TParam, TResult> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static void Join<TValue>(this IPropertyStepCaller<TValue> caller, IPropertyStep<TValue> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        public static IEventStepCaller<THandler> JoinPoint<THandler>(this IEventStepCaller<THandler> caller, out IEventStep<THandler> joinPoint)
            where THandler : Delegate
        {
            var joinStep = new MedialEventStep<THandler>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static IIndexerStepCaller<TKey, TValue> JoinPoint<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller,
            out IIndexerStep<TKey, TValue> joinPoint)
        {
            var joinStep = new MedialIndexerStep<TKey, TValue>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static IMethodStepCaller<TParam, TResult> JoinPoint<TParam, TResult>(this IMethodStepCaller<TParam, TResult> caller,
            out IMethodStep<TParam, TResult> joinPoint)
        {
            var joinStep = new MedialMethodStep<TParam, TResult>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        public static IPropertyStepCaller<TValue> JoinPoint<TValue>(this IPropertyStepCaller<TValue> caller, out IPropertyStep<TValue> joinPoint)
        {
            var joinStep = new MedialPropertyStep<TValue>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        #endregion

        #region 'Lambda' steps

        public static void Func<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, Func<TKey, TValue> func)
        {
            caller.SetNextStep(new FuncIndexerStep<TKey, TValue>(func));
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

        #endregion

        #region 'Log' Steps

        public static IEventStepCaller<THandler> Log<THandler>(this IEventStepCaller<THandler> caller, ILogContext logContext = null)
            where THandler : Delegate
        {
            return caller.SetNextStep(new LogEventStep<THandler>(logContext ?? TextWriterLogContext.Console));
        }

        public static IIndexerStepCaller<TKey, TValue> Log<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogIndexerStep<TKey, TValue>(logContext ?? TextWriterLogContext.Console));
        }

        public static IMethodStepCaller<TParam, TResult> Log<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller, ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogMethodStep<TParam, TResult>(logContext ?? TextWriterLogContext.Console));
        }

        public static IPropertyStepCaller<TValue> Log<TValue>(this IPropertyStepCaller<TValue> caller, ILogContext logContext = null)
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

        public static void StoredWithChangeNotification<TValue>(
            this IPropertyStepCaller<TValue> caller,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent,
            TValue initialValue = default,
            IEqualityComparer<TValue> comparer = null)
        {
            caller
                .OnlySetIfChanged(comparer)
                .RaisePropertyChangedEvent(propertyChangedEvent)
                .Stored(initialValue);
        }

        public static void StoredWithChangeNotification<TValue>(
            this IPropertyStepCaller<TValue> caller,
            out StoredPropertyStep<TValue> storedPropertyStep,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent,
            TValue initialValue = default,
            IEqualityComparer<TValue> comparer = null)
        {
            caller
                .OnlySetIfChanged(comparer)
                .RaisePropertyChangedEvent(propertyChangedEvent)
                .Stored(out storedPropertyStep, initialValue);
        }

        #endregion

        #region 'Missing' steps

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

        #endregion

        #region 'Return' steps

        public static void Return<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, TValue value)
        {
            caller.SetNextStep(new ReturnIndexerStep<TKey, TValue>(value));
        }

        public static IIndexerStepCaller<TKey, TValue> ReturnOnce<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, TValue value)
        {
            return caller.SetNextStep(new ReturnOnceIndexerStep<TKey, TValue>(value));
        }

        public static IIndexerStepCaller<TKey, TValue> ReturnEach<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller,
            IEnumerable<TValue> values)
        {
            return caller.SetNextStep(new ReturnEachIndexerStep<TKey, TValue>(values));
        }

        public static void Return<TParam, TResult>(this IMethodStepCaller<TParam, TResult> caller, TResult result)
        {
            caller.SetNextStep(new ReturnMethodStep<TParam, TResult>(result));
        }

        public static IMethodStepCaller<TParam, TResult> ReturnOnce<TParam, TResult>(this IMethodStepCaller<TParam, TResult> caller, TResult result)
        {
            return caller.SetNextStep(new ReturnOnceMethodStep<TParam, TResult>(result));
        }

        public static IMethodStepCaller<TParam, TResult> ReturnEach<TParam, TResult>(this IMethodStepCaller<TParam, TResult> caller,
            IEnumerable<TResult> results)
        {
            return caller.SetNextStep(new ReturnEachMethodStep<TParam, TResult>(results));
        }

        public static void Return<TValue>(this IPropertyStepCaller<TValue> caller, TValue value)
        {
            caller.SetNextStep(new ReturnPropertyStep<TValue>(value));
        }

        public static IPropertyStepCaller<TValue> ReturnOnce<TValue>(this IPropertyStepCaller<TValue> caller, TValue value)
        {
            return caller.SetNextStep(new ReturnOncePropertyStep<TValue>(value));
        }

        public static IPropertyStepCaller<TValue> ReturnEach<TValue>(this IPropertyStepCaller<TValue> caller, IEnumerable<TValue> values)
        {
            return caller.SetNextStep(new ReturnEachPropertyStep<TValue>(values));
        }

        #endregion

        #region 'Stored' steps

        public static void Stored<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            caller.SetNextStep(new StoredEventStep<THandler>());
        }

        public static void Stored<THandler>(
            this IEventStepCaller<THandler> caller,
            out StoredEventStep<THandler> step) where THandler : Delegate
        {
            step = new StoredEventStep<THandler>();
            caller.SetNextStep(step);
        }

        public static void Stored<TArgs>(this IEventStepCaller<EventHandler<TArgs>> caller, out StoredGenericEventStep<TArgs> step)
        {
            step = new StoredGenericEventStep<TArgs>();
            caller.SetNextStep(step);
        }

        public static void Stored<TValue>(
            this IPropertyStepCaller<TValue> caller,
            TValue initialValue = default)
        {
            caller.SetNextStep(
                new StoredPropertyStep<TValue>(initialValue));
        }

        public static void Stored<TValue>(
            this IPropertyStepCaller<TValue> caller,
            out StoredPropertyStep<TValue> step,
            TValue initialValue = default)
        {
            step = new StoredPropertyStep<TValue>(initialValue);
            caller.SetNextStep(step);
        }

        public static void StoredAsDictionary<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            caller.SetNextStep(new StoredAsDictionaryIndexerStep<TKey, TValue>());
        }

        public static void StoredAsDictionary<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out StoredAsDictionaryIndexerStep<TKey, TValue> step)
        {
            step = new StoredAsDictionaryIndexerStep<TKey, TValue>();
            caller.SetNextStep(step);
        }

        #endregion
    }
}
