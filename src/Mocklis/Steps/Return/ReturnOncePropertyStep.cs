// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOncePropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;

    #endregion

    public class ReturnOncePropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly TValue _value;
        private int _returnCount;

        public ReturnOncePropertyStep(TValue value)
        {
            _value = value;
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            if (Interlocked.Exchange(ref _returnCount, 1) == 0)
            {
                return _value;
            }

            return base.Get(mockInfo);
        }
    }
}
