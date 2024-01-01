// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Throw;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'throw' steps to an existing mock or step.
    /// </summary>
    public static class ThrowStepExtensions
    {
        private static Func<object, Exception> AddInstanceParameter(Func<Exception> exceptionFactory)
        {
            if (exceptionFactory == null)
            {
                throw new ArgumentNullException(nameof(exceptionFactory));
            }

            return _ => exceptionFactory();
        }

        private static Func<object, T, Exception> AddInstanceParameter<T>(Func<T, Exception> exceptionFactory)
        {
            if (exceptionFactory == null)
            {
                throw new ArgumentNullException(nameof(exceptionFactory));
            }

            return (_, t) => exceptionFactory(t);
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever an event handler is added or removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">A Func that creates the exception to be thrown. Takes the event handler as parameter.</param>
        public static void Throw<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Func<THandler?, Exception> exceptionFactory) where THandler : Delegate
        {
            caller.InstanceThrow(AddInstanceParameter(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever an event handler is added or removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">
        ///     A Func that creates the exception to be thrown. Takes the mocked instance and event
        ///     handler as parameters.
        /// </param>
        public static void InstanceThrow<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Func<object, THandler?, Exception> exceptionFactory) where THandler : Delegate
        {
            caller.SetNextStep(new ThrowEventStep<THandler>(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever a value is written to or read from the indexer.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">A Func that creates the exception to be thrown. Takes the indexer key as parameter.</param>
        public static void Throw<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<TKey, Exception> exceptionFactory)
        {
            caller.InstanceThrow(AddInstanceParameter(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever a value is written to or read from the indexer.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">
        ///     A Func that creates the exception to be thrown. Takes the mocked instance and indexer
        ///     key as parameters.
        /// </param>
        public static void InstanceThrow<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<object, TKey, Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowIndexerStep<TKey, TValue>(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever the method is called.
        /// </summary>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">A Func that creates the exception to be thrown.</param>
        public static void Throw<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<Exception> exceptionFactory)
        {
            caller.InstanceThrow(AddInstanceParameter(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever the method is called.
        /// </summary>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">A Func that creates the exception to be thrown. Takes the mocked instance as parameter.</param>
        public static void InstanceThrow<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<object, Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowMethodStep<TResult>(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever the method is called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">
        ///     A Func that creates the exception to be thrown. Takes the parameters sent to the method
        ///     as parameter.
        /// </param>
        public static void Throw<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<TParam, Exception> exceptionFactory)
        {
            caller.InstanceThrow(AddInstanceParameter(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever the method is called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">
        ///     A Func that creates the exception to be thrown. Takes the mocked instance and parameters sent to the method
        ///     as parameters.
        /// </param>
        public static void InstanceThrow<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<object, TParam, Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowMethodStep<TParam, TResult>(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever a value is written to or read from the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">A Func that creates the exception to be thrown.</param>
        public static void Throw<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<Exception> exceptionFactory)
        {
            caller.InstanceThrow(AddInstanceParameter(exceptionFactory));
        }

        /// <summary>
        ///     Introduces a step that will throw an exception whenever a value is written to or read from the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'throw' step is added.</param>
        /// <param name="exceptionFactory">A Func that creates the exception to be thrown. Takes the mocked instance as parameter.</param>
        public static void InstanceThrow<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<object, Exception> exceptionFactory)
        {
            caller.SetNextStep(new ThrowPropertyStep<TValue>(exceptionFactory));
        }
    }
}
