// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Throw' property step, that will throw and exception whenever the property is read from or
    ///     written to.
    ///     Implements the <see cref="IPropertyStep{TValue}" />
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="IPropertyStep{TValue}" />
    public class ThrowPropertyStep<TValue> : IPropertyStep<TValue>
    {
        private readonly Func<Exception> _exceptionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThrowPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="exceptionFactory">A Func that creates the exception to be thrown.</param>
        public ThrowPropertyStep(Func<Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        /// <summary>
        ///     Called when a value is read from the property. This implementation creates and throws an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public TValue Get(IMockInfo mockInfo)
        {
            throw _exceptionFactory();
        }

        /// <summary>
        ///     Called when a value is written to the property. This implementation creates and throws an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public void Set(IMockInfo mockInfo, TValue value)
        {
            throw _exceptionFactory();
        }
    }
}
