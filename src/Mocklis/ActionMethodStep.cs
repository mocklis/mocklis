// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class ActionMethodStep<TParam> : IMethodStep<TParam, ValueTuple>
    {
        private readonly Action<TParam> _action;

        public ActionMethodStep(Action<TParam> action)
        {
            _action = action;
        }

        public ValueTuple Call(MemberMock memberMock, TParam param)
        {
            _action(param);
            return ValueTuple.Create();
        }
    }

    public class ActionMethodStep : IMethodStep<ValueTuple, ValueTuple>
    {
        private readonly Action _action;

        public ActionMethodStep(Action action)
        {
            _action = action;
        }

        public ValueTuple Call(MemberMock memberMock, ValueTuple param)
        {
            _action();
            return ValueTuple.Create();
        }
    }
}
