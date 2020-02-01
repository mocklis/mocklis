// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Missing' method step. This class cannot be inherited.
    ///     Implements the <see cref="IMethodStep{TParam,TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IMethodStep{TParam, TResult}" />
    public sealed class MissingMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        /// <summary>
        ///     The singleton <see cref="MissingMethodStep{TParam,TResult}" /> instance for this type of mocked methods.
        /// </summary>
        /// <remarks>
        ///     We can use a singleton for this step as it's both final and keeps no state.
        /// </remarks>
        public static readonly MissingMethodStep<TParam, TResult> Instance = new MissingMethodStep<TParam, TResult>();

        private MissingMethodStep()
        {
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation will throw a <see cref="MockMissingException" />.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            throw new MockMissingException(MockType.Method, mockInfo);
        }
    }
}
