// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Throw' method step, that will throw an exception whenever called.
    ///     Implements the <see cref="IMethodStep{TParam,TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IMethodStep{ValueTuple, TResult}" />
    public class ThrowMethodStep<TResult> : IMethodStep<ValueTuple, TResult>
    {
        private readonly Func<object, Exception> _exceptionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThrowMethodStep{TResult}" /> class.
        /// </summary>
        /// <param name="exceptionFactory">A Func that creates the exception to be thrown. Takes the mocked instance as parameter.</param>
        public ThrowMethodStep(Func<object, Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation creates and throws an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public TResult Call(IMockInfo mockInfo, ValueTuple param)
        {
            throw _exceptionFactory(mockInfo.MockInstance);
        }
    }

    /// <summary>
    ///     Class that represents a 'Throw' method step, that will throw an exception whenever called.
    ///     Implements the <see cref="IMethodStep{TParam, TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IMethodStep{TParam, TResult}" />
    public class ThrowMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private readonly Func<object, TParam, Exception> _exceptionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThrowMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="exceptionFactory">
        ///     A Func that creates the exception to be thrown. Takes the mocked instance and parameters sent to the method
        ///     as parameters.
        /// </param>
        public ThrowMethodStep(Func<object, TParam, Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation creates and throws an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            throw _exceptionFactory(mockInfo.MockInstance, param);
        }
    }
}
