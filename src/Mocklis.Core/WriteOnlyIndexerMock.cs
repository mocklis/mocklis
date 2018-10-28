// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteOnlyIndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class WriteOnlyIndexerMock<TKey, TValue> : MemberMock, IWriteOnlyIndexerMock<TKey, TValue>
    {
        public IIndexerImplementation<TKey, TValue> Implementation { get; private set; } = MissingIndexerImplementation<TKey, TValue>.Instance;

        public WriteOnlyIndexerMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public IWriteOnlyIndexerMock<TKey, TValue> Use(IIndexerImplementation<TKey, TValue> implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            return this;
        }

        public void Set(TKey key, TValue value)
        {
            Implementation.Set(this, key, value);
        }
    }
}
