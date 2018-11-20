// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceIfMethodStep<TParam, TResult> : IfMethodStepBase<TParam, TResult>
    {
        private readonly Func<object, TParam, bool> _condition;

        public InstanceIfMethodStep(Func<object, TParam, bool> condition,
            Action<IMethodStepCaller<TParam, TResult>, IMethodStep<TParam, TResult>> ifBranch) : base(ifBranch)
        {
            _condition = condition;
        }

        public override TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return _condition(instance, param) ? IfBranch.Call(instance, memberMock, param) : base.Call(instance, memberMock, param);
        }
    }
}
