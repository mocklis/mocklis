// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodImplementation.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IMethodImplementation<in TParam, out TResult>
    {
        TResult Call(MemberMock memberMock, TParam param);
    }
}
