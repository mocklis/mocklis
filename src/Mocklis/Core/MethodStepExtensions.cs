// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Extension methods for the IMethodStep interface.
    /// </summary>
    public static class MethodStepExtensions
    {
        /// <summary>
        ///     Calls a method using a method step. If the step is <c>null</c>, uses the strictness of the mock
        ///     to decide whether to throw a <see cref="MockMissingException" /> (VeryStrict) or to return a
        ///     default value (Lenient or Strict).
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="methodStep">The method step (can be null) through which the method is called.</param>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        /// <seealso cref="IMethodStep{TParam, TResult}" />
        public static TResult CallWithStrictnessCheckIfNull<TParam, TResult>(this IMethodStep<TParam, TResult>? methodStep, IMockInfo mockInfo,
            TParam param)
        {
            if (methodStep == null)
            {
                if (mockInfo.Strictness != Strictness.VeryStrict)
                {
                    return default!;
                }

                throw new MockMissingException(MockType.Method, mockInfo);
            }

            return methodStep.Call(mockInfo, param);
        }
    }
}
