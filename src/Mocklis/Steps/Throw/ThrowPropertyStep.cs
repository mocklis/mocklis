// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowPropertyStep<TValue> : IPropertyStep<TValue>
    {
        private readonly Func<Exception> _exceptionFactory;

        public ThrowPropertyStep(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public TValue Get(IMockInfo mockInfo)
        {
            throw _exceptionFactory();
        }

        public void Set(IMockInfo mockInfo, TValue value)
        {
            throw _exceptionFactory();
        }
    }
}
