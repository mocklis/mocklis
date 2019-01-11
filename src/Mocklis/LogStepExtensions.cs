// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogStepExtensions.cs">
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

    public static class LogStepExtensions
    {
        public static ICanHaveNextEventStep<THandler> Log<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            ILogContext logContext = null) where THandler : Delegate
        {
            return caller.SetNextStep(new LogEventStep<THandler>(logContext ?? TextWriterLogContext.Console));
        }

        public static ICanHaveNextIndexerStep<TKey, TValue> Log<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogIndexerStep<TKey, TValue>(logContext ?? TextWriterLogContext.Console));
        }

        public static ICanHaveNextMethodStep<TParam, TResult> Log<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogMethodStep<TParam, TResult>(logContext ?? TextWriterLogContext.Console));
        }

        public static ICanHaveNextPropertyStep<TValue> Log<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogPropertyStep<TValue>(logContext ?? TextWriterLogContext.Console));
        }
    }
}
