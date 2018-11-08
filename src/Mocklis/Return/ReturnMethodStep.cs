// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class ReturnMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>, IFinalStep
    {
        private readonly TResult _result;

        public ReturnMethodStep(TResult result)
        {
            _result = result;
        }

        public TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return _result;
        }
    }
}
