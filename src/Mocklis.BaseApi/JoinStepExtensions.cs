// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JoinStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'join' steps to an existing mock or step.
    /// </summary>
    public static class JoinStepExtensions
    {
        /// <summary>
        ///     Introduces a step that will forward adding and removing of event handlers to another step.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'join' step is added.</param>
        /// <param name="joinPoint">The step to which adding and removing of event handlers will be forwarded.</param>
        public static void Join<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            IEventStep<THandler> joinPoint) where THandler : Delegate
        {
            caller.SetNextStep(joinPoint);
        }

        /// <summary>
        ///     Introduces a step that will forward getting and setting indexer values to another step.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'join' step is added.</param>
        /// <param name="joinPoint">The step to which getting and setting indexer values will be forwarded.</param>
        public static void Join<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            IIndexerStep<TKey, TValue> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        /// <summary>
        ///     Introduces a step that will forward method execution to another step.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'join' step is added.</param>
        /// <param name="joinPoint">The step to which method execution will be forwarded.</param>
        public static void Join<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            IMethodStep<TParam, TResult> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        /// <summary>
        ///     Introduces a step that will forward getting and setting property values to another step.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'join' step is added.</param>
        /// <param name="joinPoint">The step to which getting and setting property values will be forwarded.</param>
        public static void Join<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            IPropertyStep<TValue> joinPoint)
        {
            caller.SetNextStep(joinPoint);
        }

        /// <summary>
        ///     Introduces a step whose only purpose is to be joined to from another step. It forwards all event handler adds and
        ///     removes.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'join' step is added.</param>
        /// <param name="joinPoint">A reference to this step that can be used in a Join step.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> JoinPoint<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            out IEventStep<THandler> joinPoint)
            where THandler : Delegate
        {
            var joinStep = new EventStepWithNext<THandler>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        /// <summary>
        ///     Introduces a step whose only purpose is to be joined to from another step. It forwards all indexer reads and
        ///     writes.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'join' step is added.</param>
        /// <param name="joinPoint">A reference to this step that can be used in a Join step.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> JoinPoint<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            out IIndexerStep<TKey, TValue> joinPoint)
        {
            var joinStep = new IndexerStepWithNext<TKey, TValue>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        /// <summary>
        ///     Introduces a step whose only purpose is to be joined to from another step. It forwards all method calls.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'join' step is added.</param>
        /// <param name="joinPoint">A reference to this step that can be used in a Join step.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> JoinPoint<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out IMethodStep<TParam, TResult> joinPoint)
        {
            var joinStep = new MethodStepWithNext<TParam, TResult>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }

        /// <summary>
        ///     Introduces a step whose only purpose is to be joined to from another step. It forwards all property reads and
        ///     writes.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'join' step is added.</param>
        /// <param name="joinPoint">A reference to this step that can be used in a Join step.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> JoinPoint<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            out IPropertyStep<TValue> joinPoint)
        {
            var joinStep = new PropertyStepWithNext<TValue>();
            joinPoint = joinStep;
            return caller.SetNextStep(joinStep);
        }
    }
}
