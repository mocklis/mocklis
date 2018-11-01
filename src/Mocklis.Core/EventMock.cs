// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class EventMock<THandler> : MemberMock, IEventStepCaller<THandler> where THandler : Delegate
    {
        public IEventStep<THandler> NextStep { get; private set; } = MissingEventStep<THandler>.Instance;

        public EventMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public TStep SetNextStep<TStep>(TStep step) where TStep : IEventStep<THandler>
        {
            NextStep = step;
            return step;
        }

        public void Add(THandler value)
        {
            NextStep.Add(this, value);
        }

        public void Remove(THandler value)
        {
            NextStep.Remove(this, value);
        }
    }
}
