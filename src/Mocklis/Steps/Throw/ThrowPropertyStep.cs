// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

        public TValue Get(object instance, MemberMock memberMock)
        {
            throw _exceptionFactory();
        }

        public void Set(object instance, MemberMock memberMock, TValue value)
        {
            throw _exceptionFactory();
        }
    }
}
