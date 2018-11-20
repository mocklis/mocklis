// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnSetPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowOnSetPropertyStep<TValue> : MedialPropertyStep<TValue>
    {
        private readonly Func<Exception> _exceptionFactory;

        public ThrowOnSetPropertyStep(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public override void Set(object instance, MemberMock memberMock, TValue value)
        {
            throw _exceptionFactory();
        }
    }
}
