// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class MissingMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        public static readonly MissingMethodStep<TParam, TResult> Instance = new MissingMethodStep<TParam, TResult>();

        private MissingMethodStep()
        {
        }

        public TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            throw new MockMissingException(MockType.Method, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }
    }
}
