// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class PropertyMock<TValue> : MemberMock, IPropertyStepCaller<TValue>
    {
        public IPropertyStep<TValue> NextStep { get; private set; } = MissingPropertyStep<TValue>.Instance;

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

            NextStep = step;
            return step;
        }

        public TValue Value
        {
            get => NextStep.Get(MockInstance, this);
            set => NextStep.Set(MockInstance, this, value);
        }
    }
}
