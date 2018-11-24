// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOnceMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;

    #endregion

    public class ReturnOnceMethodStep<TParam, TResult> : MedialMethodStep<TParam, TResult>
    {
        private readonly TResult _result;
        private int _returnCount;

        public ReturnOnceMethodStep(TResult result)
        {
            _result = result;
        }

        public override TResult Call(MemberMock memberMock, TParam param)
        {
            if (Interlocked.Exchange(ref _returnCount, 1) == 0)
            {
                return _result;
            }

            return base.Call(memberMock, param);
        }
    }
}
