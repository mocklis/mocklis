// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceActionMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceActionMethodStep<TParam> : IMethodStep<TParam, ValueTuple>
    {
        private readonly Action<object, TParam> _action;

        public InstanceActionMethodStep(Action<object, TParam> action)
        {
            _action = action;
        }

        public ValueTuple Call(IMockInfo mockInfo, TParam param)
        {
            _action(mockInfo.MockInstance, param);
            return ValueTuple.Create();
        }
    }

    public class InstanceActionMethodStep : IMethodStep<ValueTuple, ValueTuple>
    {
        private readonly Action<object> _action;

        public InstanceActionMethodStep(Action<object> action)
        {
            _action = action;
        }

        public ValueTuple Call(IMockInfo mockInfo, ValueTuple param)
        {
            _action(mockInfo.MockInstance);
            return ValueTuple.Create();
        }
    }
}
