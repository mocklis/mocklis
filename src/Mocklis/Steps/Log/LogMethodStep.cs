// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public sealed class LogMethodStep<TParam, TResult> : MedialMethodStep<TParam, TResult>
    {
        private readonly ILogContext _logContext;
        private readonly bool _hasParameters;
        private readonly bool _hasResult;

        public LogMethodStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
            _hasParameters = typeof(TParam) != typeof(ValueTuple);
            _hasResult = typeof(TResult) != typeof(ValueTuple);
        }

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
