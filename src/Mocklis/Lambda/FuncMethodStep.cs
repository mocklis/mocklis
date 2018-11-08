// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class FuncMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>, IFinalStep
    {
        private readonly Func<TParam, TResult> _func;

        public FuncMethodStep(Func<TParam, TResult> func)
        {
            _func = func;
        }

        public TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return _func(param);
        }
    }

    public class FuncMethodStep<TResult> : IMethodStep<ValueTuple, TResult>, IFinalStep
    {
        private readonly Func<TResult> _func;

        public FuncMethodStep(Func<TResult> func)
        {
            _func = func;
        }

        public TResult Call(object instance, MemberMock memberMock, ValueTuple param)
        {
            return _func();
        }
    }
}
