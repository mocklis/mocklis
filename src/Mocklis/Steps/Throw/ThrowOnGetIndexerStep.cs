// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnGetIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowOnGetIndexerStep<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        private readonly Func<TKey, Exception> _exceptionFactory;

        public ThrowOnGetIndexerStep(Func<TKey, Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public override TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            throw _exceptionFactory(key);
        }
    }
}
