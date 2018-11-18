// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnGetPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowOnGetPropertyStep<TValue> : MedialPropertyStep<TValue>
    {
        private readonly Func<Exception> _exceptionFactory;

        public ThrowOnGetPropertyStep(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public override TValue Get(object instance, MemberMock memberMock)
        {
            throw _exceptionFactory();
        }
    }
}
