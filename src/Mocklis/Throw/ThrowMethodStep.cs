// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private readonly Func<TParam, Exception> _exceptionFactory;

        public ThrowMethodStep(Func<TParam, Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            throw _exceptionFactory(param);
        }
    }
}
