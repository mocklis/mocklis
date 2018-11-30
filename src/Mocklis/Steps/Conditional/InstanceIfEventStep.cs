// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceIfEventStep<THandler> : IfEventStepBase<THandler> where THandler : Delegate
    {
        private readonly Func<object, THandler, bool> _addCondition;
        private readonly Func<object, THandler, bool> _removeCondition;

        public InstanceIfEventStep(Func<object, THandler, bool> addCondition, Func<object, THandler, bool> removeCondition,
            Action<IfBranchCaller> branch) :
            base(branch)
        {
            _addCondition = addCondition;
            _removeCondition = removeCondition;
        }

        public override void Add(IMockInfo mockInfo, THandler value)
        {
            if (_addCondition?.Invoke(mockInfo.MockInstance, value) ?? false)
            {
                IfBranch.Add(mockInfo, value);
            }
            else
            {
                base.Add(mockInfo, value);
            }
        }

        public override void Remove(IMockInfo mockInfo, THandler value)
        {
            if (_removeCondition?.Invoke(mockInfo.MockInstance, value) ?? false)
            {
                IfBranch.Remove(mockInfo, value);
            }
            else
            {
                base.Remove(mockInfo, value);
            }
        }
    }
}
