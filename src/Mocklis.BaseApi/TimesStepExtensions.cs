// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Times;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'times' steps to an existing mock or step.
    /// </summary>
    public static class TimesStepExtensions
    {
        /// <summary>
        ///     Introduces an alternative branch that is used in lieu of the normal branch for a given number of uses.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="times">The number of times the alternative branch should be used.</param>
        /// <param name="branch">An action to set up the alternative branch.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> Times<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            int times,
            Action<ICanHaveNextEventStep<THandler>> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new TimesEventStep<THandler>(times, branch));
        }

        /// <summary>
        ///     Introduces an alternative branch that is used in lieu of the normal branch for a given number of uses.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="times">The number of times the alternative branch should be used.</param>
        /// <param name="branch">An action to set up the alternative branch.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> Times<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            int times,
            Action<ICanHaveNextIndexerStep<TKey, TValue>> branch)
        {
            return caller.SetNextStep(new TimesIndexerStep<TKey, TValue>(times, branch));
        }

        /// <summary>
        ///     Introduces an alternative branch that is used in lieu of the normal branch for a given number of uses.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="times">The number of times the alternative branch should be used.</param>
        /// <param name="branch">An action to set up the alternative branch.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> Times<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            int times,
            Action<ICanHaveNextMethodStep<TParam, TResult>> branch)
        {
            return caller.SetNextStep(new TimesMethodStep<TParam, TResult>(times, branch));
        }

        /// <summary>
        ///     Introduces an alternative branch that is used in lieu of the normal branch for a given number of uses.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="times">The number of times the alternative branch should be used.</param>
        /// <param name="branch">An action to set up the alternative branch.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> Times<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            int times,
            Action<ICanHaveNextPropertyStep<TValue>> branch)
        {
            return caller.SetNextStep(new TimesPropertyStep<TValue>(times, branch));
        }
    }
}
