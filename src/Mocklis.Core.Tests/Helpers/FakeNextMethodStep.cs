// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeNextMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class FakeNextMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private readonly TResult _result;
        private readonly object _lockObject = new object();
        public int Count { get; private set; }
        public IMockInfo? LastMockInfo { get; private set; }
        public TParam LastParam { get; private set; } = default!;

        public FakeNextMethodStep(ICanHaveNextMethodStep<TParam, TResult> mock, TResult result)
        {
            _result = result;
            mock.SetNextStep(this);
        }

        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            lock (_lockObject)
            {
                Count++;
                LastMockInfo = mockInfo;
                LastParam = param;
                return _result;
            }
        }
    }
}
