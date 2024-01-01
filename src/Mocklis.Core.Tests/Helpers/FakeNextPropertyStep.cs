// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeNextPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class FakeNextPropertyStep<TValue> : IPropertyStep<TValue>
    {
        private readonly TValue _value;
        private readonly object _lockObject = new object();
        public int GetCount { get; private set; }
        public IMockInfo? LastGetMockInfo { get; private set; }
        public int SetCount { get; private set; }
        public IMockInfo? LastSetMockInfo { get; private set; }
        public TValue LastSetValue { get; private set; } = default!;

        public FakeNextPropertyStep(ICanHaveNextPropertyStep<TValue> mock, TValue value)
        {
            _value = value;
            mock.SetNextStep(this);
        }

        public TValue Get(IMockInfo mockInfo)
        {
            lock (_lockObject)
            {
                GetCount++;
                LastGetMockInfo = mockInfo;
                return _value;
            }
        }

        public void Set(IMockInfo mockInfo, TValue value)
        {
            lock (_lockObject)
            {
                SetCount++;
                LastSetMockInfo = mockInfo;
                LastSetValue = value;
            }
        }
    }
}
