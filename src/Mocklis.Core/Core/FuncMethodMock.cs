// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodMock.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class FuncMethodMock<TParam, TResult> : MethodMockBase<TParam, TResult>
    {
        public FuncMethodMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        public new TResult Call(TParam param)
        {
            return base.Call(param);
        }
    }

    public sealed class FuncMethodMock<TResult> : MethodMockBase<ValueTuple, TResult>
    {
        public FuncMethodMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        public TResult Call()
        {
            return base.Call(ValueTuple.Create());
        }
    }
}
