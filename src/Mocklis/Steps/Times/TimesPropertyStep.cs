// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class TimesPropertyStep<TValue> : MedialPropertyStep<TValue>
    {
        private readonly object _lockObject = new object();
        private readonly int _times;
        private int _calls;
        private readonly MedialPropertyStep<TValue> _branch = new MedialPropertyStep<TValue>();

        public TimesPropertyStep(int times, Action<IPropertyStepCaller<TValue>> branch)
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

        public override TValue Get(IMockInfo mockInfo)
        {
            return ShouldUseBranch() ? _branch.Get(mockInfo) : base.Get(mockInfo);
        }

        public override void Set(IMockInfo mockInfo, TValue value)
        {
            if (ShouldUseBranch())
            {
                _branch.Set(mockInfo, value);
            }
            else
            {
                base.Set(mockInfo, value);
            }
        }
    }
}
