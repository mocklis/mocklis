// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Steps.Record;

    #endregion

    public static class RecordStepExtensions
    {
        public static IEventStepCaller<THandler> InstanceRecordBeforeAdd<THandler, TRecord>(
            this IEventStepCaller<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeAddEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IEventStepCaller<THandler> InstanceRecordBeforeRemove<THandler, TRecord>(
            this IEventStepCaller<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeRemoveEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IEventStepCaller<THandler> RecordBeforeAdd<THandler, TRecord>(
            this IEventStepCaller<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new RecordBeforeAddEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IEventStepCaller<THandler> RecordBeforeRemove<THandler, TRecord>(
            this IEventStepCaller<THandler> caller,
            out IReadOnlyList<TRecord> ledger, Func<THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new RecordBeforeRemoveEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IIndexerStepCaller<TKey, TValue> InstanceRecordAfterGet<TKey, TValue, TRecord>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TKey, TValue, TRecord> selection,
            Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterGetIndexerStep<TKey, TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IIndexerStepCaller<TKey, TValue> InstanceRecordBeforeSet<TKey, TValue, TRecord>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TKey, TValue, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IIndexerStepCaller<TKey, TValue> RecordAfterGet<TKey, TValue, TRecord>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TKey, TValue, TRecord> selection,
            Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterGetIndexerStep<TKey, TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IIndexerStepCaller<TKey, TValue> RecordBeforeSet<TKey, TValue, TRecord>(
            this IIndexerStepCaller<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TKey, TValue, TRecord> selection)
        {
            var newStep = new RecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IMethodStepCaller<TParam, TResult> InstanceRecordAfterCall<TParam, TResult, TRecord>(
            this IMethodStepCaller<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TParam, TResult, TRecord> selection,
            Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterCallMethodStep<TParam, TResult, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IMethodStepCaller<TParam, TResult> InstanceRecordBeforeCall<TParam, TResult, TRecord>(
            this IMethodStepCaller<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TParam, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeCallMethodStep<TParam, TResult, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IMethodStepCaller<TParam, TResult> RecordAfterCall<TParam, TResult, TRecord>(
            this IMethodStepCaller<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TParam, TResult, TRecord> selection,
            Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterCallMethodStep<TParam, TResult, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IMethodStepCaller<TParam, TResult> RecordBeforeCall<TParam, TResult, TRecord>(
            this IMethodStepCaller<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TParam, TRecord> selection)
        {
            var newStep = new RecordBeforeCallMethodStep<TParam, TResult, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IPropertyStepCaller<TValue> InstanceRecordAfterGet<TValue, TRecord>(
            this IPropertyStepCaller<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TValue, TRecord> selection,
            Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterGetPropertyStep<TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IPropertyStepCaller<TValue> InstanceRecordBeforeSet<TValue, TRecord>(
            this IPropertyStepCaller<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TValue, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeSetPropertyStep<TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IPropertyStepCaller<TValue> RecordAfterGet<TValue, TRecord>(
            this IPropertyStepCaller<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TValue, TRecord> selection,
            Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterGetPropertyStep<TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static IPropertyStepCaller<TValue> RecordBeforeSet<TValue, TRecord>(
            this IPropertyStepCaller<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TValue, TRecord> selection)
        {
            var newStep = new RecordBeforeSetPropertyStep<TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }
    }
}
