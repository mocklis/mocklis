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

        public override TResult Call(IMockInfo mockInfo, ValueTuple param)
        {
            return _condition(mockInfo.MockInstance) ? IfBranch.Call(mockInfo, param) : base.Call(mockInfo, param);
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

        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            return _condition(mockInfo.MockInstance, param) ? IfBranch.Call(mockInfo, param) : base.Call(mockInfo, param);
        }
    }
}
