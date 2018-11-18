// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredAsDictionaryIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Stored
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Verification;

    #endregion

    public class StoredAsDictionaryIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>, IStoredIndexer<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        public IDictionary<TKey, TValue> Dictionary => _dictionary;

        public TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }

            return default;
        }

        public void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            _dictionary[key] = value;
        }
    }
}
