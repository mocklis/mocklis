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

    public class InstanceIfMethodStep<TResult> : IfMethodStepBase<ValueTuple, TResult>
    {
        private readonly Func<object, bool> _condition;

        public InstanceIfMethodStep(Func<object, bool> condition,
            Action<IfBranchCaller> branch) : base(branch)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public override TResult Call(object instance, MemberMock memberMock, ValueTuple param)
        {
            return _condition(instance) ? IfBranch.Call(instance, memberMock, param) : base.Call(instance, memberMock, param);
        }
    }

    public class InstanceIfMethodStep<TParam, TResult> : IfMethodStepBase<TParam, TResult>
    {
        private readonly Func<object, TParam, bool> _condition;

        public InstanceIfMethodStep(Func<object, TParam, bool> condition,
            Action<IfBranchCaller> branch) : base(branch)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public override TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return _condition(instance, param) ? IfBranch.Call(instance, memberMock, param) : base.Call(instance, memberMock, param);
        }
    }
}
