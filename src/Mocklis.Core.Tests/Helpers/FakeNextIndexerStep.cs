// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeNextIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Helpers
{
    public class FakeNextIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        private readonly TValue _value;
        private readonly object _lockObject = new object();
        public int GetCount { get; private set; }
        public IMockInfo LastGetMockInfo { get; private set; }
        public TKey LastGetKey { get; private set; }
        public int SetCount { get; private set; }
        public IMockInfo LastSetMockInfo { get; private set; }
        public TKey LastSetKey { get; private set; }
        public TValue LastSetValue { get; private set; }

        public FakeNextIndexerStep(ICanHaveNextIndexerStep<TKey, TValue> mock, TValue value)
        {
            _value = value;
            mock.SetNextStep(this);
        }

        public TValue Get(IMockInfo mockInfo, TKey key)
        {
            lock (_lockObject)
            {
                GetCount++;
                LastGetMockInfo = mockInfo;
                LastGetKey = key;
                return _value;
            }
        }

        public void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            lock (_lockObject)
            {
                SetCount++;
                LastSetMockInfo = mockInfo;
                LastSetKey = key;
                LastSetValue = value;
            }
        }
    }
}
