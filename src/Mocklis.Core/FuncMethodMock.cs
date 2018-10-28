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
        public FuncMethodMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public TResult Call(TParam param)
        {
            return Call(this, param);
        }
    }

    public sealed class FuncMethodMock<TResult> : BaseMethodMock<ValueTuple, TResult>
    {
        public FuncMethodMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public TResult Call()
        {
            return Call(this, ValueTuple.Create());
        }
    }
}
