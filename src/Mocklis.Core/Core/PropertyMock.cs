// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public sealed class PropertyMock<TValue> : MemberMock, IPropertyStepCaller<TValue>
    {
        private IPropertyStep<TValue> _nextStep = MissingPropertyStep<TValue>.Instance;

        public PropertyMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        public TStep SetNextStep<TStep>(TStep step) where TStep : IPropertyStep<TValue>
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            _nextStep = step;
            return step;
        }

        public TValue Value
        {
            get => _nextStep.Get(this);
            set => _nextStep.Set(this, value);
        }
    }
}
