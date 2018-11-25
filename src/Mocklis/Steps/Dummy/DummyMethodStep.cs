// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class DummyMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        public static readonly DummyMethodStep<TParam, TResult> Instance = new DummyMethodStep<TParam, TResult>();

        private DummyMethodStep()
        {
        }

        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            return default;
        }
    }
}
