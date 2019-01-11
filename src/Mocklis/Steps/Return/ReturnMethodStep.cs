// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class ReturnMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private readonly TResult _result;

        public ReturnMethodStep(TResult result)
        {
            _result = result;
        }

        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            return _result;
        }
    }
}
