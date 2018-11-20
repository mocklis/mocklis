// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnRemoveEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowOnRemoveEventStep<THandler> : MedialEventStep<THandler> where THandler : Delegate
    {
        private readonly Func<Exception> _exceptionFactory;

        public ThrowOnRemoveEventStep(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public override void Remove(object instance, MemberMock memberMock, THandler value)
        {
            throw _exceptionFactory();
        }
    }
}
