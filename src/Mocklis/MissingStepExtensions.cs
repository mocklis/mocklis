// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Missing;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'missing' steps to an existing mock or step.
    /// </summary>
    public static class MissingStepExtensions
    {
        /// <summary>
        ///     Introduces a step that will throw a <see cref="MockMissingException" /> whenever an event handler is added or
        ///     removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'missing' step is added.</param>
        public static void Missing<THandler>(
            this ICanHaveNextEventStep<THandler> caller) where THandler : Delegate
        {
            caller.SetNextStep(MissingEventStep<THandler>.Instance);
        }

        /// <summary>
        ///     Introduces a step that will throw a <see cref="MockMissingException" /> whenever the indexer is read from or
        ///     written to.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'missing' step is added.</param>
        public static void Missing<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller)
        {
            caller.SetNextStep(MissingIndexerStep<TKey, TValue>.Instance);
        }

        /// <summary>
        ///     Introduces a step that will throw a <see cref="MockMissingException" /> whenever the method is called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'missing' step is added.</param>
        public static void Missing<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller)
        {
            caller.SetNextStep(MissingMethodStep<TParam, TResult>.Instance);
        }

        /// <summary>
        ///     Introduces a step that will throw a <see cref="MockMissingException" /> whenever the property is read from or
        ///     written to.
        /// </summary>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <param name="caller">The mock or step to which this 'missing' step is added.</param>
        public static void Missing<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller)
        {
            caller.SetNextStep(MissingPropertyStep<TValue>.Instance);
        }
    }
}
