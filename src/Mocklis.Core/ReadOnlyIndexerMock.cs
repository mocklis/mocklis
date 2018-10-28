// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyIndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class ReadOnlyIndexerMock<TKey, TValue> : MemberMock, IReadOnlyIndexerMock<TKey, TValue>
    {
        public IIndexerImplementation<TKey, TValue> Implementation { get; private set; } = MissingIndexerImplementation<TKey, TValue>.Instance;

        public ReadOnlyIndexerMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public IReadOnlyIndexerMock<TKey, TValue> Use(IIndexerImplementation<TKey, TValue> implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            return this;
        }

        public TValue Get(TKey key)
        {
            return Implementation.Get(this, key);
        }
    }
}
