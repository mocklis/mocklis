// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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

    /// <summary>
    ///     A class with extension methods for adding 'record' steps to an existing mock or step.
    /// </summary>
    public static class RecordStepExtensions
    {
        /// <summary>
        ///     Introduces a step that will record an entry before an event handler is added.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock and the event as
        ///     parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> InstanceRecordBeforeAdd<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeAddEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry before an event handler is removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock and the event as
        ///     parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> InstanceRecordBeforeRemove<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeRemoveEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry before an event handler is added.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">A Func that selects what we want to record. Takes the event as parameter.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> RecordBeforeAdd<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new RecordBeforeAddEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry before an event handler is removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">A Func that selects what we want to record. Takes the event as parameter.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> RecordBeforeRemove<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger, Func<THandler, TRecord> selection) where THandler : Delegate
        {
            var newStep = new RecordBeforeRemoveEventStep<THandler, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry after a value is read from the indexer; optionally also recording
        ///     exceptions.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock, the indexer key
        ///     and the read value as parameters.
        /// </param>
        /// <param name="onError">
        ///     An optional Func that constructs an entry when an exception was thrown by a subsequent step.
        ///     Takes the entire state of the mock and the exception as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
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

        /// <summary>
        ///     Introduces a step that will record an entry before a value as been written to the indexer.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock, the indexer key
        ///     and the value to be written as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceRecordBeforeSet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TKey, TValue, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry after a value is read from the indexer; optionally also recording
        ///     exceptions.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the indexer key and the read value as
        ///     parameters.
        /// </param>
        /// <param name="onError">
        ///     An optional Func that constructs an entry when an exception was thrown by a subsequent step.
        ///     Takes the exception as parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
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

        /// <summary>
        ///     Introduces a step that will record an entry before a value as been written to the indexer.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the indexer key and the value to be written
        ///     as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> RecordBeforeSet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TKey, TValue, TRecord> selection)
        {
            var newStep = new RecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry when a method has been called; optionally also recording exceptions.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock, the parameters
        ///     sent to the method and the returned value as parameters.
        /// </param>
        /// <param name="onError">
        ///     An optional Func that constructs an entry when an exception was thrown by a subsequent step.
        ///     Takes the entire state of the mock and the exception as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
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

        /// <summary>
        ///     Introduces a step that will record an entry before a method is about to be called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock and the
        ///     parameters sent to the method as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> InstanceRecordBeforeCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TParam, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeCallMethodStep<TParam, TResult, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry when a method has been called; optionally also recording exceptions.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the parameters sent to the method and the
        ///     returned value as parameters.
        /// </param>
        /// <param name="onError">
        ///     An optional Func that constructs an entry when an exception was thrown by a subsequent step.
        ///     Takes the exception as parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
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

        /// <summary>
        ///     Introduces a step that will record an entry before a method is about to be called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the parameters sent to the method as
        ///     parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> RecordBeforeCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TParam, TRecord> selection)
        {
            var newStep = new RecordBeforeCallMethodStep<TParam, TResult, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry after a value is read from the property; optionally also recording
        ///     exceptions.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock and the read
        ///     value as parameters.
        /// </param>
        /// <param name="onError">
        ///     An optional Func that constructs an entry when an exception was thrown by a subsequent step.
        ///     Takes the entire state of the mock and the exception as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
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

        /// <summary>
        ///     Introduces a step that will record an entry before a value as been written to the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock and the value to
        ///     be written as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> InstanceRecordBeforeSet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TValue, TRecord> selection)
        {
            var newStep = new InstanceRecordBeforeSetPropertyStep<TValue, TRecord>(selection);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry after a value is read from the property; optionally also recording
        ///     exceptions.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">A Func that selects what we want to record. Takes the read value as parameter.</param>
        /// <param name="onError">
        ///     An optional Func that constructs an entry when an exception was thrown by a subsequent step.
        ///     Takes the exception as parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
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

        /// <summary>
        ///     Introduces a step that will record an entry before a value as been written to the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selection">A Func that selects what we want to record. Takes the value to be written as parameter.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
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
