// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Log;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'log' steps to an existing mock or step.
    /// </summary>
    public static class LogStepExtensions
    {
        /// <summary>
        ///     Introduces a step that logs all adds and removes of event handlers to the mocked event to a log context, or the
        ///     console if none was provided.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="logContext">
        ///     The <see cref="ILogContext" /> used to write the log entries. The default will write to the
        ///     console.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> Log<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            ILogContext logContext = null) where THandler : Delegate
        {
            return caller.SetNextStep(new LogEventStep<THandler>(logContext ?? WriteLineLogContext.Console));
        }

        /// <summary>
        ///     Introduces a step that logs all adds and removes of event handlers to the mocked event to a log context, where
        ///     the log context is provided by an <see cref="ILogContextProvider" />.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="logContextProvider">An instance from which we can get an <see cref="ILogContext" /> to use.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> Log<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            ILogContextProvider logContextProvider) where THandler : Delegate
        {
            if (logContextProvider == null)
            {
                throw new ArgumentNullException(nameof(logContextProvider));
            }

            return caller.SetNextStep(new LogEventStep<THandler>(logContextProvider.LogContext));
        }

        /// <summary>
        ///     Introduces a step that logs all gets and sets of the mocked indexer to a log context, or the console if none was
        ///     provided.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="logContext">
        ///     The <see cref="ILogContext" /> used to write the log entries. The default will write to the
        ///     console.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> Log<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogIndexerStep<TKey, TValue>(logContext ?? WriteLineLogContext.Console));
        }

        /// <summary>
        ///     Introduces a step that logs all gets and sets of the mocked indexer to a log context, where
        ///     the log context is provided by an <see cref="ILogContextProvider" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="logContextProvider">An instance from which we can get an <see cref="ILogContext" /> to use.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> Log<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            ILogContextProvider logContextProvider)
        {
            if (logContextProvider == null)
            {
                throw new ArgumentNullException(nameof(logContextProvider));
            }

            return caller.SetNextStep(new LogIndexerStep<TKey, TValue>(logContextProvider.LogContext));
        }

        /// <summary>
        ///     Introduces a step that logs all calls to the mocked method to a log context, or the console if none was provided.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="logContext">
        ///     The <see cref="ILogContext" /> used to write the log entries. The default will write to the
        ///     console.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> Log<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogMethodStep<TParam, TResult>(logContext ?? WriteLineLogContext.Console));
        }

        /// <summary>
        ///     Introduces a step that logs all calls to the mocked method to a log context, where
        ///     the log context is provided by an <see cref="ILogContextProvider" />.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="logContextProvider">An instance from which we can get an <see cref="ILogContext" /> to use.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> Log<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            ILogContextProvider logContextProvider)
        {
            if (logContextProvider == null)
            {
                throw new ArgumentNullException(nameof(logContextProvider));
            }

            return caller.SetNextStep(new LogMethodStep<TParam, TResult>(logContextProvider.LogContext));
        }

        /// <summary>
        ///     Introduces a step that logs all gets and sets of the mocked property to a log context, where
        ///     the log context is provided by an <see cref="ILogContextProvider" />.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="logContext">
        ///     The <see cref="ILogContext" /> used to write the log entries. The default will write to the
        ///     console.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> Log<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogPropertyStep<TValue>(logContext ?? WriteLineLogContext.Console));
        }

        /// <summary>
        ///     Introduces a step that logs all gets and sets of the mocked property to a log context, or the console if none was
        ///     provided.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'log' step is added.</param>
        /// <param name="logContextProvider">An instance from which we can get an <see cref="ILogContext" /> to use.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> Log<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            ILogContextProvider logContextProvider)
        {
            if (logContextProvider == null)
            {
                throw new ArgumentNullException(nameof(logContextProvider));
            }

            return caller.SetNextStep(new LogPropertyStep<TValue>(logContextProvider.LogContext));
        }
    }
}
