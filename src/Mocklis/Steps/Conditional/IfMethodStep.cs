// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfMethodStep<TResult> : IfMethodStepBase<ValueTuple, TResult>
    {
        private readonly Func<bool> _condition;

        public IfMethodStep(Func<bool> condition, Action<IfBranchCaller> branch) : base(branch)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public override TResult Call(object instance, MemberMock memberMock, ValueTuple param)
        {
            return _condition() ? IfBranch.Call(instance, memberMock, param) : base.Call(instance, memberMock, param);
        }
    }

    public class IfMethodStep<TParam, TResult> : IfMethodStepBase<TParam, TResult>
    {
        private readonly Func<TParam, bool> _condition;

        public IfMethodStep(Func<TParam, bool> condition, Action<IfBranchCaller> branch) : base(branch)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public override TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return _condition(param) ? IfBranch.Call(instance, memberMock, param) : base.Call(instance, memberMock, param);
        }
    }
}
