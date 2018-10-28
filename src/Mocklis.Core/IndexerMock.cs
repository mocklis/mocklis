// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class IndexerMock<TKey, TValue> : MemberMock, IIndexerMock<TKey, TValue>
    {
        public IIndexerImplementation<TKey, TValue> Implementation { get; private set; } = MissingIndexerImplementation<TKey, TValue>.Instance;

        public IndexerMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public IIndexerMock<TKey, TValue> Use(IIndexerImplementation<TKey, TValue> implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            return this;
        }

        public TValue Get(TKey key)
        {
            return Implementation.Get(this, key);
        }

        public void Set(TKey key, TValue value)
        {
            Implementation.Set(this, key, value);
        }
    }
}
