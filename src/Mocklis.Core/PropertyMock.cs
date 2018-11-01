// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class PropertyMock<TValue> : MemberMock, IPropertyStepCaller<TValue>
    {
        public IPropertyStep<TValue> NextStep { get; private set; } = MissingPropertyStep<TValue>.Instance;

        public PropertyMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public TStep SetNextStep<TStep>(TStep step) where TStep : IPropertyStep<TValue>
        {
            NextStep = step;
            return step;
        }

        public TValue Get()
        {
            return NextStep.Get(this);
        }

        public void Set(TValue value)
        {
            NextStep.Set(this, value);
        }
    }
}
