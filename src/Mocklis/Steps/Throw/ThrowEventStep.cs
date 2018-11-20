// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowEventStep<THandler> : IEventStep<THandler> where THandler : Delegate
    {
        private readonly Func<Exception> _exceptionFactory;

        public ThrowEventStep(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public void Add(object instance, MemberMock memberMock, THandler value)
        {
            throw _exceptionFactory();
        }

        public void Remove(object instance, MemberMock memberMock, THandler value)
        {
            throw _exceptionFactory();
        }
    }
}
