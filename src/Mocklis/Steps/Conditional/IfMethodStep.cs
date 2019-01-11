// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
            _condition = condition;
        }

        public override TResult Call(IMockInfo mockInfo, ValueTuple param)
        {
            if (_condition?.Invoke() ?? false)
            {
                return IfBranch.Call(mockInfo, param);
            }

            return base.Call(mockInfo, param);
        }
    }

    public class IfMethodStep<TParam, TResult> : IfMethodStepBase<TParam, TResult>
    {
        private readonly Func<TParam, bool> _condition;

        public IfMethodStep(Func<TParam, bool> condition, Action<IfBranchCaller> branch) : base(branch)
        {
            _condition = condition;
        }

        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            if (_condition?.Invoke(param) ?? false)
            {
                return IfBranch.Call(mockInfo, param);
            }

            return base.Call(mockInfo, param);
        }
    }
}
