// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeNextEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class FakeNextEventStep<THandler> : IEventStep<THandler> where THandler : Delegate
    {
        private readonly object _lockObject = new object();
        public int AddCount { get; private set; }
        public IMockInfo? LastAddMockInfo { get; private set; }
        public THandler? LastAddValue { get; private set; }
        public int RemoveCount { get; private set; }
        public IMockInfo? LastRemoveMockInfo { get; private set; }
        public THandler? LastRemoveValue { get; private set; }

        public FakeNextEventStep(ICanHaveNextEventStep<THandler> mock)
        {
            mock.SetNextStep(this);
        }

        public void Add(IMockInfo mockInfo, THandler? value)
        {
            lock (_lockObject)
            {
                AddCount++;
                LastAddMockInfo = mockInfo;
                LastAddValue = value;
            }
        }

        public void Remove(IMockInfo mockInfo, THandler? value)
        {
            lock (_lockObject)
            {
                RemoveCount++;
                LastRemoveMockInfo = mockInfo;
                LastRemoveValue = value;
            }
        }
    }
}
