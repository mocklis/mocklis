// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowMethodStep<TResult> : IMethodStep<ValueTuple, TResult>
    {
        private readonly Func<Exception> _exceptionFactory;

        public ThrowMethodStep(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public TResult Call(IMockInfo mockInfo, ValueTuple param)
        {
            throw _exceptionFactory();
        }
    }

    public class ThrowMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private readonly Func<TParam, Exception> _exceptionFactory;

        public ThrowMethodStep(Func<TParam, Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            throw _exceptionFactory(param);
        }
    }
}
