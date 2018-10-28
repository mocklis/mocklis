// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriteOnlyIndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IWriteOnlyIndexerMock<TKey, TValue>
    {
        IIndexerImplementation<TKey, TValue> Implementation { get; }
        IWriteOnlyIndexerMock<TKey, TValue> Use(IIndexerImplementation<TKey, TValue> implementation);
    }
}
