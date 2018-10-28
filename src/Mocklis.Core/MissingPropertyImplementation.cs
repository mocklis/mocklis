// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingPropertyImplementation.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class MissingPropertyImplementation<TValue> : IPropertyImplementation<TValue>
    {
        public static readonly MissingPropertyImplementation<TValue> Instance = new MissingPropertyImplementation<TValue>();

        private MissingPropertyImplementation()
        {
        }

        public TValue Get(MemberMock memberMock)
        {
            throw new MockMissingException(MockType.PropertyGet, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }

        public void Set(MemberMock memberMock, TValue value)
        {
            throw new MockMissingException(MockType.PropertySet, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }
    }
}
