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
    using System.Threading;
    using Mocklis.Conditional;
    using Mocklis.Core;
    using Mocklis.Dummy;
    using Mocklis.Lambda;
    using Mocklis.Log;
    using Mocklis.Miscellaneous;
    using Mocklis.Record;
    using Mocklis.Return;
    using Mocklis.Stored;
    using Mocklis.Verification;

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

        public static void InstanceFunc<TKey, TValue>(this IIndexerStepCaller<TKey, TValue> caller, Func<object, TKey, TValue> func)
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

        public static void InstanceFunc<TValue>(this IPropertyStepCaller<TValue> caller, Func<object, TValue> func)
        {
            caller.SetNextStep(new InstanceFuncPropertyStep<TValue>(func));
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

        #region 'Record' steps

        public static IEventStepCaller<THandler> InstanceRecordBeforeAdd<THandler, TRecord>(this IEventStepCaller<THandler> caller,
            out IReadOnlyList<TRecord> ledger, Func<object, THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeAddEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IEventStepCaller<THandler> InstanceRecordBeforeRemove<THandler, TRecord>(this IEventStepCaller<THandler> caller,
            out IReadOnlyList<TRecord> ledger, Func<object, THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeRemoveEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IEventStepCaller<THandler> RecordBeforeAdd<THandler, TRecord>(this IEventStepCaller<THandler> caller,
            out IReadOnlyList<TRecord> ledger, Func<THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new RecordBeforeAddEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IEventStepCaller<THandler> RecordBeforeRemove<THandler, TRecord>(this IEventStepCaller<THandler> caller,
            out IReadOnlyList<TRecord> ledger, Func<THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new RecordBeforeRemoveEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IIndexerStepCaller<TKey, TValue> InstanceRecordAfterGet<TKey, TValue, TRecord>(this IIndexerStepCaller<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger, Func<object, TKey, TValue, TRecord> selection, Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterGetIndexerStep<TKey, TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IIndexerStepCaller<TKey, TValue> InstanceRecordBeforeSet<TKey, TValue, TRecord>(this IIndexerStepCaller<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger, Func<object, TKey, TValue, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IIndexerStepCaller<TKey, TValue> RecordAfterGet<TKey, TValue, TRecord>(this IIndexerStepCaller<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger, Func<TKey, TValue, TRecord> selection, Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterGetIndexerStep<TKey, TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IIndexerStepCaller<TKey, TValue> RecordBeforeSet<TKey, TValue, TRecord>(this IIndexerStepCaller<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger, Func<TKey, TValue, TRecord> selection)
        {
            var newStep = new RecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IMethodStepCaller<TParam, TResult> InstanceRecordAfterCall<TParam, TResult, TRecord>(
            this IMethodStepCaller<TParam, TResult> caller, out IReadOnlyList<TRecord> ledger, Func<object, TParam, TResult, TRecord> selection,
            Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterCallMethodStep<TParam, TResult, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IMethodStepCaller<TParam, TResult> InstanceRecordBeforeCall<TParam, TResult, TRecord>(
            this IMethodStepCaller<TParam, TResult> caller, out IReadOnlyList<TRecord> ledger, Func<object, TParam, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeCallMethodStep<TParam, TResult, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IMethodStepCaller<TParam, TResult> RecordAfterCall<TParam, TResult, TRecord>(this IMethodStepCaller<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger, Func<TParam, TResult, TRecord> selection, Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterCallMethodStep<TParam, TResult, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IMethodStepCaller<TParam, TResult> RecordBeforeCall<TParam, TResult, TRecord>(this IMethodStepCaller<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger, Func<TParam, TRecord> selection)
        {
            var newStep = new RecordBeforeCallMethodStep<TParam, TResult, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IPropertyStepCaller<TValue> InstanceRecordAfterGet<TValue, TRecord>(this IPropertyStepCaller<TValue> caller,
            out IReadOnlyList<TRecord> ledger, Func<object, TValue, TRecord> selection, Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterGetPropertyStep<TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IPropertyStepCaller<TValue> InstanceRecordBeforeSet<TValue, TRecord>(this IPropertyStepCaller<TValue> caller,
            out IReadOnlyList<TRecord> ledger, Func<object, TValue, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeSetPropertyStep<TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IPropertyStepCaller<TValue> RecordAfterGet<TValue, TRecord>(this IPropertyStepCaller<TValue> caller,
            out IReadOnlyList<TRecord> ledger, Func<TValue, TRecord> selection, Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterGetPropertyStep<TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IPropertyStepCaller<TValue> RecordBeforeSet<TValue, TRecord>(this IPropertyStepCaller<TValue> caller,
            out IReadOnlyList<TRecord> ledger, Func<TValue, TRecord> selection)
        {
            var newStep = new RecordBeforeSetPropertyStep<TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
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

        public static IStoredEvent<THandler> Stored<THandler>(
            this IEventStepCaller<THandler> caller) where THandler : Delegate
        {
            return caller.SetNextStep(new StoredEventStep<THandler>());
        }

        public static IStoredEvent<THandler> Stored<THandler>(
            this IEventStepCaller<THandler> caller,
            out StoredEventStep<THandler> step) where THandler : Delegate
        {
            step = new StoredEventStep<THandler>();
            return caller.SetNextStep(step);
        }

        public static IStoredEvent<EventHandler<TArgs>> Stored<TArgs>(this IEventStepCaller<EventHandler<TArgs>> caller,
            out StoredGenericEventStep<TArgs> step)
        {
            step = new StoredGenericEventStep<TArgs>();
            return caller.SetNextStep(step);
        }

        public static IStoredProperty<TValue> Stored<TValue>(
            this IPropertyStepCaller<TValue> caller,
            TValue initialValue = default)
        {
            return caller.SetNextStep(new StoredPropertyStep<TValue>(initialValue));
        }

        public static IStoredProperty<TValue> Stored<TValue>(
            this IPropertyStepCaller<TValue> caller,
            out StoredPropertyStep<TValue> step,
            TValue initialValue = default)
        {
            step = new StoredPropertyStep<TValue>(initialValue);
            return caller.SetNextStep(step);
        }

        public static IStoredIndexer<TKey, TValue> StoredAsDictionary<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller)
        {
            return caller.SetNextStep(new StoredAsDictionaryIndexerStep<TKey, TValue>());
        }

        public static IStoredIndexer<TKey, TValue> StoredAsDictionary<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out StoredAsDictionaryIndexerStep<TKey, TValue> step)
        {
            step = new StoredAsDictionaryIndexerStep<TKey, TValue>();
            return caller.SetNextStep(step);
        }

        #endregion
    }
}
