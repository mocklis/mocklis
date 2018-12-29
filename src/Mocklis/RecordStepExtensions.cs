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
        public static ICanHaveNextEventStep<THandler> InstanceRecordBeforeAdd<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeAddEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextEventStep<THandler> InstanceRecordBeforeRemove<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeRemoveEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextEventStep<THandler> RecordBeforeAdd<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new RecordBeforeAddEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextEventStep<THandler> RecordBeforeRemove<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger, Func<THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new RecordBeforeRemoveEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceRecordAfterGet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TKey, TValue, TRecord> selection,
            Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterGetIndexerStep<TKey, TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceRecordBeforeSet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TKey, TValue, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> RecordAfterGet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TKey, TValue, TRecord> selection,
            Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterGetIndexerStep<TKey, TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> RecordBeforeSet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TKey, TValue, TRecord> selection)
        {
            var newStep = new RecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextMethodStep<TParam, TResult> InstanceRecordAfterCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TParam, TResult, TRecord> selection,
            Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterCallMethodStep<TParam, TResult, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextMethodStep<TParam, TResult> InstanceRecordBeforeCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TParam, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeCallMethodStep<TParam, TResult, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextMethodStep<TParam, TResult> RecordAfterCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TParam, TResult, TRecord> selection,
            Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterCallMethodStep<TParam, TResult, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextMethodStep<TParam, TResult> RecordBeforeCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TParam, TRecord> selection)
        {
            var newStep = new RecordBeforeCallMethodStep<TParam, TResult, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextPropertyStep<TValue> InstanceRecordAfterGet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TValue, TRecord> selection,
            Func<object, Exception, TRecord> onError = null)
        {
            var newStep = new InstanceRecordAfterGetPropertyStep<TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextPropertyStep<TValue> InstanceRecordBeforeSet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TValue, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeSetPropertyStep<TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextPropertyStep<TValue> RecordAfterGet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TValue, TRecord> selection,
            Func<Exception, TRecord> onError = null)
        {
            var newStep = new RecordAfterGetPropertyStep<TValue, TRecord>(selection, onError);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        public static ICanHaveNextPropertyStep<TValue> RecordBeforeSet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TValue, TRecord> selection)
        {
            var newStep = new RecordBeforeSetPropertyStep<TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }
    }
}
