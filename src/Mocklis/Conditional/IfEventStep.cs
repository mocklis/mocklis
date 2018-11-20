// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfEventStep<THandler> : IfEventStepBase<THandler> where THandler : Delegate
    {
        private readonly Func<THandler, bool> _addCondition;
        private readonly Func<THandler, bool> _removeCondition;

        public IfEventStep(Func<THandler, bool> addCondition, Func<THandler, bool> removeCondition,
            Action<IEventStepCaller<THandler>, IEventStep<THandler>> ifBranchRemoveup) :
            base(ifBranchRemoveup)
        {
            _addCondition = addCondition ?? throw new ArgumentNullException(nameof(addCondition));
            _removeCondition = removeCondition ?? throw new ArgumentNullException(nameof(removeCondition));
        }

        public override void Add(object instance, MemberMock memberMock, THandler value)
        {
            if (_addCondition(value))
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
            if (_removeCondition(value))
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
