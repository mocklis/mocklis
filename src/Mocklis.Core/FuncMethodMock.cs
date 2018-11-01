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

        public TResult Call(object instance, TParam param)
        {
            return Call(instance, this, param);
        }
    }

    public sealed class FuncMethodMock<TResult> : BaseMethodMock<ValueTuple, TResult>
    {
        public FuncMethodMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public TResult Call(object instance)
        {
            return Call(instance, this, ValueTuple.Create());
        }
    }
}
