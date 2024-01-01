// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Dummy' method step. This class cannot be inherited.
    ///     Implements the <see cref="IMethodStep{TParam,TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IMethodStep{TParam, TResult}" />
    public sealed class DummyMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        /// <summary>
        ///     The singleton <see cref="DummyMethodStep{TParam, TResult}" /> instance for this type of mocked methods.
        /// </summary>
        /// <remarks>
        ///     We can use a singleton for this step as it's both final and keeps no state.
        /// </remarks>
        public static readonly DummyMethodStep<TParam, TResult> Instance = new DummyMethodStep<TParam, TResult>();

        private DummyMethodStep()
        {
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation will ignore any parameters and return a default value.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            return default!;
        }
    }
}
