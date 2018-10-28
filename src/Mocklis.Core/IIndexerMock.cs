// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IIndexerMock<TKey, TValue>
    {
        IIndexerImplementation<TKey, TValue> Implementation { get; }
        IIndexerMock<TKey, TValue> Use(IIndexerImplementation<TKey, TValue> implementation);
    }
}
