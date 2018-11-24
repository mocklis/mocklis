// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfEventStepBase.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    /*
     * This class looks like it's more complicated than it needs to be. The key is that
     * we need to have three separate implementations of the IEventStep<THandler> interface.
     * (1) The step itself (IfEventStepBase and derived classes), (2) one that
     * represents the if-branch (IfBranchCaller), so that classes deriving from IfEventStepBase
     * can direct calls to it, and (3) one that can be used to rejoin the original branch
     * (ElseBranchRejoiner). This needs to be separate from (1), since we need to bypass the
     * if-logic if we use it.
     *
     * ElseBranchRejoiner needs to be an inner class to IfEventStepBase to have visibility
     * on its NextStep property. 
     *
     * IfBranchCaller needs to be public, since it needs to expose IEventStepCaller<THandler>
     * and the ElseBranch Property through the IfBranch property of the IfEventStepBase class.
     * If the latter returned IEventStepCaller we wouldn't see the ElseBranch, and if it
     * returned an interface incorporating both then the extension methods that connect up
     * further steps would stop working. It needs to be an inner class to IfBranchCaller to
     * be able to construct the ElseBranchRejoiner for its ElseBranch property.
     *
     * Lastly, while the IfBranchCaller has much code in common with MedialEventStep
     * we prefer to have an explicit declaration of the IEventStep<THandler> interface,
     * otherwise the Add and Remove methods would show up in intellisense when setting up
     * the steps for the if branch.
     *
     * All in all this is the simplest thing that could work. Pity it isn't particularly simple.
     */

    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Missing;

    #endregion

    public abstract class IfEventStepBase<THandler> : MedialEventStep<THandler> where THandler : Delegate
    {
        public sealed class IfBranchCaller : IEventStep<THandler>, IEventStepCaller<THandler>
        {
            private IEventStep<THandler> _nextStep = MissingEventStep<THandler>.Instance;

            public TStep SetNextStep<TStep>(TStep step) where TStep : IEventStep<THandler>
            {
                if (step == null)
                {
                    throw new ArgumentNullException(nameof(step));
                }

                _nextStep = step;
                return step;
            }

            void IEventStep<THandler>.Add(MemberMock memberMock, THandler value)
            {
                _nextStep.Add(memberMock, value);
            }

            void IEventStep<THandler>.Remove(MemberMock memberMock, THandler value)
            {
                _nextStep.Remove(memberMock, value);
            }

            public IEventStep<THandler> ElseBranch { get; }

            public IfBranchCaller(IfEventStepBase<THandler> ifEventStep)
            {
                ElseBranch = new ElseBranchRejoiner(ifEventStep);
            }
        }

        private sealed class ElseBranchRejoiner : IEventStep<THandler>
        {
            private readonly IfEventStepBase<THandler> _ifEventStep;

            public ElseBranchRejoiner(IfEventStepBase<THandler> ifEventStep)
            {
                _ifEventStep = ifEventStep;
            }

            public void Add(MemberMock memberMock, THandler value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifEventStep.NextStep.Add(memberMock, value);
            }

            public void Remove(MemberMock memberMock, THandler value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifEventStep.NextStep.Remove(memberMock, value);
            }
        }

        protected IEventStep<THandler> IfBranch { get; }

        protected IfEventStepBase(Action<IfBranchCaller> branch)
        {
            var ifBranch = new IfBranchCaller(this);
            IfBranch = ifBranch;
            branch?.Invoke(ifBranch);
        }
    }
}