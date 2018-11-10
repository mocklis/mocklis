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

        public EventMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
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

        public void Add(object instance, THandler value)
        {
            _nextStep.Add(instance, this, value);
        }

        public void Remove(object instance, THandler value)
        {
            _nextStep.Remove(instance, this, value);
        }
    }
}
