// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Dummy' property step. This class cannot be inherited.
    ///     Implements the <see cref="IPropertyStep{TValue}" />
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="IPropertyStep{TValue}" />
    public sealed class DummyPropertyStep<TValue> : IPropertyStep<TValue>
    {
        /// <summary>
        ///     The singleton <see cref="DummyPropertyStep{TValue}" /> instance for this type of mocked properties.
        /// </summary>
        /// <remarks>
        ///     We can use a singleton for this step as it's both final and keeps no state.
        /// </remarks>
        public static readonly DummyPropertyStep<TValue> Instance = new DummyPropertyStep<TValue>();

        private DummyPropertyStep()
        {
        }

        /// <summary>
        ///     Called when a value is read from the property. This implementation will return a default value.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public TValue Get(IMockInfo mockInfo)
        {
            return default;
        }

        /// <summary>
        ///     Called when a value is written to the property. This implementation will do nothing.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public void Set(IMockInfo mockInfo, TValue value)
        {
        }
    }
}
