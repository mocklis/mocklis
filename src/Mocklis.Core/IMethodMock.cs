// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IMethodMock<TParam, TResult>
    {
        IMethodImplementation<TParam, TResult> Implementation { get; }
        IMethodMock<TParam, TResult> Use(IMethodImplementation<TParam, TResult> implementation);
    }
}
