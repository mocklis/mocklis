// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadOnlyIndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IReadOnlyIndexerMock<TKey, TValue>
    {
        IIndexerImplementation<TKey, TValue> Implementation { get; }
        IReadOnlyIndexerMock<TKey, TValue> Use(IIndexerImplementation<TKey, TValue> implementation);
    }
}
