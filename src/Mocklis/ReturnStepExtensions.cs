// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Mocklis.Core;
    using Mocklis.Steps.Return;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'return' steps to an existing mock or step.
    /// </summary>
    public static class ReturnStepExtensions
    {
        /// <summary>
        ///     Introduces a step that returns a given value whenever read, while passing on any writes to subsequent steps.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="value">The value to be returned.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> Return<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnIndexerStep<TKey, TValue>(value));
        }

        /// <summary>
        ///     Introduces a step that returns a given value the first time it is read, while passing on any writes and further
        ///     reads to subsequent steps.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="value">The value to be returned.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> ReturnOnce<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnOnceIndexerStep<TKey, TValue>(value));
        }

        /// <summary>
        ///     Introduces a step that returns values from a list one-by-one when read, while passing on any writes to subsequent
        ///     steps. It will also
        ///     pass on reads once the list has been exhausted.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="values">The values to be returned one-by-one.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> ReturnEach<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            IEnumerable<TValue> values)
        {
            return caller.SetNextStep(new ReturnEachIndexerStep<TKey, TValue>(values));
        }

        /// <summary>
        ///     Introduces a step that returns values from a list one-by-one when read, while passing on any writes to subsequent
        ///     steps. It will also
        ///     pass on reads once the list has been exhausted.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="values">The values to be returned one-by-one.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> ReturnEach<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            params TValue[] values)
        {
            return caller.ReturnEach(values.AsEnumerable());
        }

        /// <summary>
        ///     Introduces a step that returns a given result whenever called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="result">The result to be returned.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static void Return<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            TResult result)
        {
            caller.SetNextStep(new ReturnMethodStep<TParam, TResult>(result));
        }

        /// <summary>
        ///     Introduces a step that returns a given result the first time it is called, while passing on any further calls to
        ///     subsequent steps.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="result">The result to be returned.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> ReturnOnce<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            TResult result)
        {
            return caller.SetNextStep(new ReturnOnceMethodStep<TParam, TResult>(result));
        }

        /// <summary>
        ///     Introduces a step that returns results from a list one-by-one when called. It will pass on calls once the list has
        ///     been exhausted.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="results">The list of results to be returned one-by-one.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> ReturnEach<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            IEnumerable<TResult> results)
        {
            return caller.SetNextStep(new ReturnEachMethodStep<TParam, TResult>(results));
        }

        /// <summary>
        ///     Introduces a step that returns results from a list one-by-one when called. It will pass on calls once the list has
        ///     been exhausted.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="results">The list of results to be returned one-by-one.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> ReturnEach<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            params TResult[] results)
        {
            return caller.ReturnEach(results.AsEnumerable());
        }

        /// <summary>
        ///     Introduces a step that returns a given value whenever read, while passing on any writes to subsequent steps.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="value">The value to be returned.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> Return<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnPropertyStep<TValue>(value));
        }

        /// <summary>
        ///     Introduces a step that returns a given value the first time it is read, while passing on any writes and further
        ///     reads to subsequent steps.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="value">The value to be returned.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> ReturnOnce<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            TValue value)
        {
            return caller.SetNextStep(new ReturnOncePropertyStep<TValue>(value));
        }

        /// <summary>
        ///     Introduces a step that returns values from a list one-by-one when read, while passing on any writes to subsequent
        ///     steps. It will also
        ///     pass on reads once the list has been exhausted.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="values">The values to be returned one-by-one.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> ReturnEach<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            IEnumerable<TValue> values)
        {
            return caller.SetNextStep(new ReturnEachPropertyStep<TValue>(values));
        }

        /// <summary>
        ///     Introduces a step that returns values from a list one-by-one when read, while passing on any writes to subsequent
        ///     steps. It will also
        ///     pass on reads once the list has been exhausted.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'return' step is added.</param>
        /// <param name="values">The values to be returned one-by-one.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> ReturnEach<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            params TValue[] values)
        {
            return caller.ReturnEach(values.AsEnumerable());
        }
    }
}
