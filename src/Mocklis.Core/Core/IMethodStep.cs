// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Interface that models things that can happen with a mocked method.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    public interface IMethodStep<in TParam, out TResult>
    {
        /// <summary>
        ///     Called when the mocked method is called.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        TResult Call(IMockInfo mockInfo, TParam param);
    }
}
