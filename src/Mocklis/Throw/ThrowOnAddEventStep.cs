// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnAddEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowOnAddEventStep<THandler> : MedialEventStep<THandler> where THandler : Delegate
    {
        private readonly Func<Exception> _exceptionFactory;

        public ThrowOnAddEventStep(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public override void Add(object instance, MemberMock memberMock, THandler value)
        {
            throw _exceptionFactory();
        }
    }
}
