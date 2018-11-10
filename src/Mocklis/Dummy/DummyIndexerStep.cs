// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Dummy
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class DummyIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        public static readonly DummyIndexerStep<TKey, TValue> Instance = new DummyIndexerStep<TKey, TValue>();

        private DummyIndexerStep()
        {
        }

        public TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return default;
        }

        public void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
        }
    }
}
