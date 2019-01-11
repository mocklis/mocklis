// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
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

        public ValueTuple Call(IMockInfo mockInfo, TParam param)
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

        public ValueTuple Call(IMockInfo mockInfo, ValueTuple param)
        {
            _action();
            return ValueTuple.Create();
        }
    }
}
