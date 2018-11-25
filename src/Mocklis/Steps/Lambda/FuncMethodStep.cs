// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class FuncMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private readonly Func<TParam, TResult> _func;

        public FuncMethodStep(Func<TParam, TResult> func)
        {
            _func = func;
        }

        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            return _func(param);
        }
    }

    public class FuncMethodStep<TResult> : IMethodStep<ValueTuple, TResult>
    {
        private readonly Func<TResult> _func;

        public FuncMethodStep(Func<TResult> func)
        {
            _func = func;
        }

        public TResult Call(IMockInfo mockInfo, ValueTuple param)
        {
            return _func();
        }
    }
}
