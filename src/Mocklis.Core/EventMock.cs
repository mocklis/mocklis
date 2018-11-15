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
        private IEventStep<THandler> _nextStep = MissingEventStep<THandler>.Instance;

        public EventMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        public TStep SetNextStep<TStep>(TStep step) where TStep : IEventStep<THandler>
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            _nextStep = step;
            return step;
        }

        public void Add(THandler value)
        {
            _nextStep.Add(MockInstance, this, value);
        }

        public void Remove(THandler value)
        {
            _nextStep.Remove(MockInstance, this, value);
        }
    }
}
