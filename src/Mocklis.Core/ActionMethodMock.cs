// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class ActionMethodMock<TParam> : BaseMethodMock<TParam, ValueTuple>
    {
        public ActionMethodMock(string interfaceName, string memberName, string memberMockName)
            : base(interfaceName, memberName, memberMockName)
        {
        }

        public void Call(object instance, TParam param)
        {
            Call(instance, this, param);
        }
    }

    public sealed class ActionMethodMock : BaseMethodMock<ValueTuple, ValueTuple>
    {
        public ActionMethodMock(string interfaceName, string memberName, string memberMockName)
            : base(interfaceName, memberName, memberMockName)
        {
        }

        public void Call(object instance)
        {
            Call(instance, this, ValueTuple.Create());
        }
    }
}
