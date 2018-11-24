// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOncePropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;

    #endregion

    public class ReturnOncePropertyStep<TValue> : MedialPropertyStep<TValue>
    {
        private readonly TValue _value;
        private int _returnCount;

        public ReturnOncePropertyStep(TValue value)
        {
            _value = value;
        }

        public override TValue Get(MemberMock memberMock)
        {
            if (Interlocked.Exchange(ref _returnCount, 1) == 0)
            {
                return _value;
            }

            return base.Get(memberMock);
        }
    }
}