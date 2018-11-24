// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IMethodStep<in TParam, out TResult>
    {
        TResult Call(MemberMock memberMock, TParam param);
    }
}
