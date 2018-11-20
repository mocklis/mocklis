// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnSetIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ThrowOnSetIndexerStep<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        private readonly Func<TKey, Exception> _exceptionFactory;

        public ThrowOnSetIndexerStep(Func<TKey, Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        public override void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            throw _exceptionFactory(key);
        }
    }
}
