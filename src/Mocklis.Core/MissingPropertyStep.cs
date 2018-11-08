// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class MissingPropertyStep<TValue> : IPropertyStep<TValue>, IFinalStep
    {
        public static readonly MissingPropertyStep<TValue> Instance = new MissingPropertyStep<TValue>();

        private MissingPropertyStep()
        {
        }

        public TValue Get(object instance, MemberMock memberMock)
        {
            throw new MockMissingException(MockType.PropertyGet, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }

        public void Set(object instance, MemberMock memberMock, TValue value)
        {
            throw new MockMissingException(MockType.PropertySet, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }
    }
}
