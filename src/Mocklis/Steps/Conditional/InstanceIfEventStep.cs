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
            _addCondition = addCondition ?? throw new ArgumentNullException(nameof(addCondition));
            _removeCondition = removeCondition ?? throw new ArgumentNullException(nameof(removeCondition));
        }

        public override void Add(object instance, MemberMock memberMock, THandler value)
        {
            if (_addCondition(instance, value))
            {
                IfBranch.Add(instance, memberMock, value);
            }
            else
            {
                base.Add(instance, memberMock, value);
            }
        }

        public override void Remove(object instance, MemberMock memberMock, THandler value)
        {
            if (_removeCondition(instance, value))
            {
                IfBranch.Remove(instance, memberMock, value);
            }
            else
            {
                base.Remove(instance, memberMock, value);
            }
        }
    }
}
