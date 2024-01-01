// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
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
        #region Event Extensions

        /// <summary>
        ///     Introduces a step that will record an entry before an event handler is added.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="selector">
        ///     A Func that constructs an entry for when an event handler is added.
        ///     Takes the mocked instance and the event handler as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> InstanceRecordBeforeAdd<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, THandler?, TRecord> selector) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeAddEventStep<THandler, TRecord>(selector);
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when an event handler is removed.
        ///     Takes the mocked instance and the event handler as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> InstanceRecordBeforeRemove<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, THandler?, TRecord> selector) where THandler : Delegate
        {
            var newStep = new InstanceRecordBeforeRemoveEventStep<THandler, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry before an event handler is added.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> RecordBeforeAdd<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<THandler?> ledger) where THandler : Delegate
        {
            var newStep = new RecordBeforeAddEventStep<THandler, THandler?>(h => h);
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when an event handler is added.
        ///     Takes the event handler as parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> RecordBeforeAdd<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<THandler?, TRecord> selector) where THandler : Delegate
        {
            var newStep = new RecordBeforeAddEventStep<THandler, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry before an event handler is removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> RecordBeforeRemove<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<THandler?> ledger) where THandler : Delegate
        {
            var newStep = new RecordBeforeRemoveEventStep<THandler, THandler?>(h => h);
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when an event handler is removed.
        ///     Takes the event handler as parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> RecordBeforeRemove<THandler, TRecord>(
            this ICanHaveNextEventStep<THandler> caller,
            out IReadOnlyList<TRecord> ledger, Func<THandler?, TRecord> selector) where THandler : Delegate
        {
            var newStep = new RecordBeforeRemoveEventStep<THandler, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        #endregion

        #region Indexer Extensions

        /// <summary>
        ///     Introduces a step that will record an entry after a value is read from the indexer; optionally also recording
        ///     exceptions.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a value has been read.
        ///     Takes the mocked instance, the key used and the value as parameters.
        /// </param>
        /// <param name="failureSelector">
        ///     An Func that constructs an entry for an exception thrown when reading a value.
        ///     Takes the mocked instance, the key used and the exception as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceRecordAfterGet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TKey, TValue, TRecord>? successSelector,
            Func<object, TKey, Exception, TRecord>? failureSelector = null)
        {
            var newStep = new InstanceRecordAfterGetIndexerStep<TKey, TValue, TRecord>(successSelector, failureSelector);
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when a value is written.
        ///     Takes the mocked instance, the key used and the value as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceRecordBeforeSet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TKey, TValue, TRecord> selector)
        {
            var newStep = new InstanceRecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry after a value is read from the indexer.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> RecordAfterGet<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<(TKey Key, TValue Value)> ledger)
        {
            var newStep = new RecordAfterGetIndexerStep<TKey, TValue, (TKey, TValue)>((k, v) => (k, v));
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
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a value has been read.
        ///     Takes the key used and the value as parameters.
        /// </param>
        /// <param name="failureSelector">
        ///     An Func that constructs an entry for an exception thrown when reading a value.
        ///     Takes the key used and the exception as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> RecordAfterGet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TKey, TValue, TRecord>? successSelector,
            Func<TKey, Exception, TRecord>? failureSelector = null)
        {
            var newStep = new RecordAfterGetIndexerStep<TKey, TValue, TRecord>(successSelector, failureSelector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry before a value as been written to the indexer.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> RecordBeforeSet<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<(TKey Key, TValue Value)> ledger)
        {
            var newStep = new RecordBeforeSetIndexerStep<TKey, TValue, (TKey, TValue)>((k, v) => (k, v));
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when a value is written.
        ///     Takes the key used and the value as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> RecordBeforeSet<TKey, TValue, TRecord>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TKey, TValue, TRecord> selector)
        {
            var newStep = new RecordBeforeSetIndexerStep<TKey, TValue, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        #endregion

        #region Method Extensions

        /// <summary>
        ///     Introduces a step that will record an entry when a method has been called; optionally also recording exceptions.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a result is returned from a call.
        ///     Takes the mocked instance, the parameters sent and the returned value as parameters.
        /// </param>
        /// <param name="failureSelector">
        ///     A Func that constructs an entry for an exception thrown by a call.
        ///     Takes the mocked instance, the parameters sent and the exception as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> InstanceRecordAfterCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TParam, TResult, TRecord>? successSelector,
            Func<object, TParam, Exception, TRecord>? failureSelector = null)
        {
            var newStep = new InstanceRecordAfterCallMethodStep<TParam, TResult, TRecord>(successSelector, failureSelector);
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when a call is made.
        ///     Takes the mocked instance and the parameters sent as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> InstanceRecordBeforeCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TParam, TRecord> selector)
        {
            var newStep = new InstanceRecordBeforeCallMethodStep<TParam, TResult, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry when a method has been called; optionally also recording exceptions.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> RecordAfterCall<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<(TParam Parameter, TResult Result)> ledger)
        {
            var newStep = new RecordAfterCallMethodStep<TParam, TResult, (TParam, TResult)>((p, r) => (p, r), null);
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
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a result is returned from a call.
        ///     Takes the parameters sent and the returned value as parameters.
        /// </param>
        /// <param name="failureSelector">
        ///     A Func that constructs an entry for an exception thrown by a call.
        ///     Takes the parameters sent and the exception as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> RecordAfterCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TParam, TResult, TRecord>? successSelector,
            Func<TParam, Exception, TRecord>? failureSelector = null)
        {
            var newStep = new RecordAfterCallMethodStep<TParam, TResult, TRecord>(successSelector, failureSelector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry before a method is about to be called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> RecordBeforeCall<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TParam> ledger)
        {
            var newStep = new RecordBeforeCallMethodStep<TParam, TResult, TParam>(p => p);
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when a call is made.
        ///     Takes the parameters sent as parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> RecordBeforeCall<TParam, TResult, TRecord>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TParam, TRecord> selector)
        {
            var newStep = new RecordBeforeCallMethodStep<TParam, TResult, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        #endregion

        #region Property Extensions

        /// <summary>
        ///     Introduces a step that will record an entry after a value is read from the property; optionally also recording
        ///     exceptions.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <typeparam name="TRecord">The type of the entries that will be recorded in the ledger.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a value has been read.
        ///     Takes the mocked instance and the value as parameters.
        /// </param>
        /// <param name="failureSelector">
        ///     An Func that constructs an entry for an exception thrown when reading a value.
        ///     Takes the mocked instance and the exception as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> InstanceRecordAfterGet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TValue, TRecord>? successSelector,
            Func<object, Exception, TRecord>? failureSelector = null)
        {
            var newStep = new InstanceRecordAfterGetPropertyStep<TValue, TRecord>(successSelector, failureSelector);
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when a value is written.
        ///     Takes the mocked instance and the value as parameters.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> InstanceRecordBeforeSet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<object, TValue, TRecord> selector)
        {
            var newStep = new InstanceRecordBeforeSetPropertyStep<TValue, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry after a value is read from the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> RecordAfterGet<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TValue> ledger)
        {
            var newStep = new RecordAfterGetPropertyStep<TValue, TValue>(v => v);
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
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a value has been read.
        ///     Takes the value as parameter.
        /// </param>
        /// <param name="failureSelector">
        ///     An Func that constructs an entry for an exception thrown when reading a value.
        ///     Takes the exception as parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> RecordAfterGet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TValue, TRecord>? successSelector,
            Func<Exception, TRecord>? failureSelector = null)
        {
            var newStep = new RecordAfterGetPropertyStep<TValue, TRecord>(successSelector, failureSelector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        /// <summary>
        ///     Introduces a step that will record an entry before a value as been written to the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'record' step is added.</param>
        /// <param name="ledger">A list that contains recorded entries.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> RecordBeforeSet<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TValue> ledger)
        {
            var newStep = new RecordBeforeSetPropertyStep<TValue, TValue>(v => v);
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
        /// <param name="selector">
        ///     A Func that constructs an entry for when a value is written.
        ///     Takes the value as parameter.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> RecordBeforeSet<TValue, TRecord>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IReadOnlyList<TRecord> ledger,
            Func<TValue, TRecord> selector)
        {
            var newStep = new RecordBeforeSetPropertyStep<TValue, TRecord>(selector);
            ledger = newStep;
            return caller.SetNextStep(newStep);
        }

        #endregion
    }
}
