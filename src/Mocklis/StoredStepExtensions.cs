// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Mocklis.Core;
    using Mocklis.Stored;
    using Mocklis.Verification;

    #endregion

    public static class StoredStepExtensions
    {
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

        public static IStoredEvent<EventHandler<TArgs>> Stored<TArgs>(
            this IEventStepCaller<EventHandler<TArgs>> caller,
            out StoredGenericEventStep<TArgs> step)
        {
            step = new StoredGenericEventStep<TArgs>();
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

        public static IStoredProperty<TValue> StoredWithChangeNotification<TValue>(
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

        public static IStoredProperty<TValue> StoredWithChangeNotification<TValue>(
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
    }
}
