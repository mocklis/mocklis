// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IMethodStep<in TParam, out TResult>
    {
        TResult Call(IMockInfo mockInfo, TParam param);
    }
}
