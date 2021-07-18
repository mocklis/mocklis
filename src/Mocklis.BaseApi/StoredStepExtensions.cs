// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Mocklis.Core;
    using Mocklis.Steps.Stored;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'stored' steps to an existing mock or step.
    /// </summary>
    public static class StoredStepExtensions
    {
        /// <summary>
        ///     Introduces a step that will remember ('store') event handlers adder or removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'stored' step is added.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static IStoredEvent<THandler> Stored<THandler>(
            this ICanHaveNextEventStep<THandler> caller) where THandler : Delegate
        {
            return caller.SetNextStep(new StoredEventStep<THandler>());
        }

        /// <summary>
        ///     Introduces a step that will remember ('store') event handlers adder or removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'stored' step is added.</param>
        /// <param name="step">
        ///     Returns the added step itself. It can be used to manipulate the store, check contents of the store,
        ///     and raise events on added handlers.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static IStoredEvent<THandler> Stored<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            out StoredEventStep<THandler> step) where THandler : Delegate
        {
            step = new StoredEventStep<THandler>();
            return caller.SetNextStep(step);
        }

        /// <summary>
        ///     Introduces a step that will remember ('store') values written to it, returning them when read. It uses a dictionary
        ///     for storage.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'stored' step is added.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static IStoredIndexer<TKey, TValue> StoredAsDictionary<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller)
        {
            return caller.SetNextStep(new StoredAsDictionaryIndexerStep<TKey, TValue>());
        }

        /// <summary>
        ///     Introduces a step that will remember ('store') values written to it, returning them when read. It uses a dictionary
        ///     for storage.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'stored' step is added.</param>
        /// <param name="step">
        ///     Returns the added step itself. It can be used to manipulate the store and check contents of the
        ///     store.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static IStoredIndexer<TKey, TValue> StoredAsDictionary<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out StoredAsDictionaryIndexerStep<TKey, TValue> step)
        {
            step = new StoredAsDictionaryIndexerStep<TKey, TValue>();
            return caller.SetNextStep(step);
        }

        /// <summary>
        ///     Introduces a step that will remember ('store') values written to it, returning them when read.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'stored' step is added.</param>
        /// <param name="initialValue">The initial value of the store, or default if none was given.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static IStoredProperty<TValue> Stored<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            TValue initialValue = default)
        {
            return caller.SetNextStep(new StoredPropertyStep<TValue>(initialValue));
        }

        /// <summary>
        ///     Introduces a step that will remember ('store') values written to it, returning them when read.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'stored' step is added.</param>
        /// <param name="step">
        ///     Returns the added step itself. It can be used to manipulate the store and check contents of the
        ///     store.
        /// </param>
        /// <param name="initialValue">The initial value of the store, or default if none was given.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static IStoredProperty<TValue> Stored<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out StoredPropertyStep<TValue> step,
            TValue initialValue = default)
        {
            step = new StoredPropertyStep<TValue>(initialValue);
            return caller.SetNextStep(step);
        }

        /// <summary>
        ///     Introduces a set of steps that mimic setting a property with event notification.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'stored' step is added.</param>
        /// <param name="propertyChangedEvent">The event step used to store <see cref="PropertyChangedEventHandler" /> instances.</param>
        /// <param name="initialValue">The initial value of the property.</param>
        /// <param name="comparer">
        ///     An optional comparer used to determine if the value of the property has changed. An event will
        ///     only be raised if it has.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static IStoredProperty<TValue> StoredWithChangeNotification<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent,
            TValue initialValue = default,
            IEqualityComparer<TValue>? comparer = null)
        {
            return caller
                .OnlySetIfChanged(comparer)
                .RaisePropertyChangedEvent(propertyChangedEvent)
                .Stored(initialValue);
        }

        /// <summary>
        ///     Introduces a set of steps that mimic setting a property with event notification.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'stored' step is added.</param>
        /// <param name="storedPropertyStep">
        ///     Returns the 'stored' property step added. It can be used to manipulate the store and
        ///     check contents of the store.
        /// </param>
        /// <param name="propertyChangedEvent">The event step used to store <see cref="PropertyChangedEventHandler" /> instances.</param>
        /// <param name="initialValue">The initial value of the property.</param>
        /// <param name="comparer">
        ///     An optional comparer used to determine if the value of the property has changed. An event will
        ///     only be raised if it has.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static IStoredProperty<TValue> StoredWithChangeNotification<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out StoredPropertyStep<TValue> storedPropertyStep,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent,
            TValue initialValue = default,
            IEqualityComparer<TValue>? comparer = null)
        {
            return caller
                .OnlySetIfChanged(comparer)
                .RaisePropertyChangedEvent(propertyChangedEvent)
                .Stored(out storedPropertyStep, initialValue);
        }
    }
}
