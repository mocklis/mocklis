// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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
        public static IEventStepCaller<THandler> Log<THandler>(
            this IEventStepCaller<THandler> caller,
            ILogContext logContext = null) where THandler : Delegate
        {
            return caller.SetNextStep(new LogEventStep<THandler>(logContext ?? TextWriterLogContext.Console));
        }

        public static IIndexerStepCaller<TKey, TValue> Log<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogIndexerStep<TKey, TValue>(logContext ?? TextWriterLogContext.Console));
        }

        public static IMethodStepCaller<TParam, TResult> Log<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogMethodStep<TParam, TResult>(logContext ?? TextWriterLogContext.Console));
        }

        public static IPropertyStepCaller<TValue> Log<TValue>(
            this IPropertyStepCaller<TValue> caller,
            ILogContext logContext = null)
        {
            return caller.SetNextStep(new LogPropertyStep<TValue>(logContext ?? TextWriterLogContext.Console));
        }
    }
}
