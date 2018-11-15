// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class FuncMethodMock<TParam, TResult> : BaseMethodMock<TParam, TResult>
    {
        public FuncMethodMock(object mockInstance, string interfaceName, string memberName, string memberMockName) : base(mockInstance, interfaceName,
            memberName, memberMockName)
        {
        }

        public new TResult Call(TParam param)
        {
            return base.Call(param);
        }
    }

    public sealed class FuncMethodMock<TResult> : BaseMethodMock<ValueTuple, TResult>
    {
        public FuncMethodMock(object mockInstance, string interfaceName, string memberName, string memberMockName) : base(mockInstance, interfaceName,
            memberName, memberMockName)
        {
        }

        public TResult Call()
        {
            return base.Call(ValueTuple.Create());
        }
    }
}
