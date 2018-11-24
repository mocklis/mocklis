// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
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
            Action<IfBranchCaller> branch) :
            base(branch)
        {
            _addCondition = addCondition ?? throw new ArgumentNullException(nameof(addCondition));
            _removeCondition = removeCondition ?? throw new ArgumentNullException(nameof(removeCondition));
        }

        public override void Add(MemberMock memberMock, THandler value)
        {
            if (_addCondition(value))
            {
                IfBranch.Add(memberMock, value);
            }
            else
            {
                base.Add(memberMock, value);
            }
        }

        public override void Remove(MemberMock memberMock, THandler value)
        {
            if (_removeCondition(value))
            {
                IfBranch.Remove(memberMock, value);
            }
            else
            {
                base.Remove(memberMock, value);
            }
        }
    }
}
