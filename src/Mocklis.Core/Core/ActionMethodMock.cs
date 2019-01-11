// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodMock.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class ActionMethodMock<TParam> : MethodMockBase<TParam, ValueTuple>
    {
        public ActionMethodMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        public new void Call(TParam param)
        {
            base.Call(param);
        }
    }

    public sealed class ActionMethodMock : MethodMockBase<ValueTuple, ValueTuple>
    {
        public ActionMethodMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        public void Call()
        {
            base.Call(ValueTuple.Create());
        }
    }
}
