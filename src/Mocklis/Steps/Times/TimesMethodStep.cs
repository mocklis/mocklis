// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class TimesMethodStep<TParam, TResult> : MedialMethodStep<TParam, TResult>
    {
        private readonly object _lockObject = new object();
        private readonly int _times;
        private int _calls;
        private readonly MedialMethodStep<TParam, TResult> _branch = new MedialMethodStep<TParam, TResult>();

        public TimesMethodStep(int times, Action<IMethodStepCaller<TParam, TResult>> branch)
        {
            _times = times;
            branch(_branch);
        }

        private bool ShouldUseBranch()
        {
            lock (_lockObject)
            {
                if (_calls < _times)
                {
                    _calls++;
                    return true;
                }
            }

            return false;
        }

        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            return ShouldUseBranch() ? _branch.Call(mockInfo, param) : base.Call(mockInfo, param);
        }
    }
}
