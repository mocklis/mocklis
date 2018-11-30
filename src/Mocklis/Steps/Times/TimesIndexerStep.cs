// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class TimesIndexerStep<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        private readonly object _lockObject = new object();
        private readonly int _times;
        private int _calls;
        private readonly MedialIndexerStep<TKey, TValue> _branch = new MedialIndexerStep<TKey, TValue>();

        public TimesIndexerStep(int times, Action<IIndexerStepCaller<TKey, TValue>> branch)
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

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return ShouldUseBranch() ? _branch.Get(mockInfo, key) : base.Get(mockInfo, key);
        }

        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            if (ShouldUseBranch())
            {
                _branch.Set(mockInfo, key, value);
            }
            else
            {
                base.Set(mockInfo, key, value);
            }
        }
    }
}
