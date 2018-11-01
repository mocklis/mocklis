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

        public void Add(object instance, THandler value)
        {
            NextStep.Add(instance, this, value);
        }

        public void Remove(object instance, THandler value)
        {
            NextStep.Remove(instance, this, value);
        }
    }
}
