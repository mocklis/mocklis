// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingMethodImplementation.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class MissingMethodImplementation<TParam, TResult> : IMethodImplementation<TParam, TResult>
    {
        public static readonly MissingMethodImplementation<TParam, TResult> Instance = new MissingMethodImplementation<TParam, TResult>();

        private MissingMethodImplementation()
        {
        }

        public TResult Call(MemberMock memberMock, TParam param)
        {
            throw new MockMissingException(MockType.Method, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }
    }
}
