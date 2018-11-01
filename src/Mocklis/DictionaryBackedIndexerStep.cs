// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryBackedIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    public class DictionaryBackedIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public IDictionary<TKey, TValue> Dictionary => _dictionary;

        public TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return _dictionary[key];
        }

        public void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            _dictionary[key] = value;
        }
    }
}
