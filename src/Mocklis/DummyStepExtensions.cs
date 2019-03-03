// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyStepExtensions.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Dummy;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'dummy' steps to an existing mock or step.
    /// </summary>
    public static class DummyStepExtensions
    {
        /// <summary>
        ///     Introduces a step that will ignore all adding and removing of event handlers.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'dummy' step is added.</param>
        public static void Dummy<THandler>(
            this ICanHaveNextEventStep<THandler> caller) where THandler : Delegate
        {
            caller.SetNextStep(DummyEventStep<THandler>.Instance);
        }

        /// <summary>
        ///     Introduces a step that will ignore all writes and return default values for all reads.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'dummy' step is added.</param>
        public static void Dummy<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller)
        {
            caller.SetNextStep(DummyIndexerStep<TKey, TValue>.Instance);
        }

        /// <summary>
        ///     Introduces a step that will ignore parameter values and just return default return values when called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'dummy' step is added.</param>
        public static void Dummy<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller)
        {
            caller.SetNextStep(DummyMethodStep<TParam, TResult>.Instance);
        }

        /// <summary>
        ///     Introduces a step that will ignore all writes and return default values for all reads.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'dummy' step is added.</param>
        public static void Dummy<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller)
        {
            caller.SetNextStep(DummyPropertyStep<TValue>.Instance);
        }
    }
}
