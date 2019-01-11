// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public sealed class EventMock<THandler> : MemberMock, ICanHaveNextEventStep<THandler> where THandler : Delegate
    {
        private IEventStep<THandler> _nextStep = MissingEventStep<THandler>.Instance;

        public EventMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        TStep ICanHaveNextEventStep<THandler>.SetNextStep<TStep>(TStep step)
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
            _nextStep.Add(this, value);
        }

        public void Remove(THandler value)
        {
            _nextStep.Remove(this, value);
        }
    }
}
