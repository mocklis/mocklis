// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'missing' property step. This class cannot be inherited.
    ///     Implements the <see cref="IPropertyStep{TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="IPropertyStep{TValue}" />
    public sealed class MissingPropertyStep<TValue> : IPropertyStep<TValue>
    {
        /// <summary>
        ///     The singleton <see cref="MissingPropertyStep{TValue}" /> instance for this type of mocked events.
        /// </summary>
        /// <remarks>
        ///     We can use a singleton for this step as it's both final and keeps no state.
        /// </remarks>
        public static readonly MissingPropertyStep<TValue> Instance = new MissingPropertyStep<TValue>();

        private MissingPropertyStep()
        {
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation will throw a <see cref="MockMissingException" />.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public TValue Get(IMockInfo mockInfo)
        {
            throw new MockMissingException(MockType.PropertyGet, mockInfo);
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     This implementation will throw a <see cref="MockMissingException" />.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public void Set(IMockInfo mockInfo, TValue value)
        {
            throw new MockMissingException(MockType.PropertySet, mockInfo);
        }
    }
}
