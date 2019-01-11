// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesEventStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class TimesEventStep<THandler> : EventStepWithNext<THandler> where THandler : Delegate
    {
        private readonly object _lockObject = new object();
        private readonly int _times;
        private int _calls;
        private readonly EventStepWithNext<THandler> _branch = new EventStepWithNext<THandler>();

        public TimesEventStep(int times, Action<ICanHaveNextEventStep<THandler>> branch)
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

        public override void Add(IMockInfo mockInfo, THandler value)
        {
            if (ShouldUseBranch())
            {
                _branch.Add(mockInfo, value);
            }
            else
            {
                base.Add(mockInfo, value);
            }
        }

        public override void Remove(IMockInfo mockInfo, THandler value)
        {
            if (ShouldUseBranch())
            {
                _branch.Remove(mockInfo, value);
            }
            else
            {
                base.Remove(mockInfo, value);
            }
        }
    }
}
