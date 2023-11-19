// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Log' method step. This class cannot be inherited.
    ///     Implements the <see cref="MethodStepWithNext{TParam,TResult}" />
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MethodStepWithNext{TParam, TResult}" />
    public sealed class LogMethodStep<TParam, TResult> : MethodStepWithNext<TParam, TResult>
    {
        private readonly ILogContext _logContext;
        private readonly bool _hasParameters;
        private readonly bool _hasResult;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="logContext">The log context used to write log lines.</param>
        public LogMethodStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
            _hasParameters = typeof(TParam) != typeof(ValueTuple);
            _hasResult = typeof(TResult) != typeof(ValueTuple);
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     THis implementation logs before and after the method has been called, along with any exceptions thrown.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            if (_hasParameters)
            {
                _logContext.LogBeforeMethodCallWithParameters(mockInfo, param);
            }
            else
            {
                _logContext.LogBeforeMethodCallWithoutParameters(mockInfo);
            }

            TResult result;

            try
            {
                result = base.Call(mockInfo, param);
            }
            catch (Exception exception)
            {
                _logContext.LogMethodCallException(mockInfo, exception);
                throw;
            }

            if (_hasResult)
            {
                _logContext.LogAfterMethodCallWithResult(mockInfo, result);
            }
            else
            {
                _logContext.LogAfterMethodCallWithoutResult(mockInfo);
            }

            return result;
        }
    }
}
