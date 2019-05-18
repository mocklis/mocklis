// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Stored
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Stored' property step, using a normal property as backing store.
    ///     Implements the <see cref="IPropertyStep{TValue}" /> interface.
    ///     Implements the <see cref="IStoredProperty{TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="IPropertyStep{TValue}" />
    /// <seealso cref="IStoredProperty{TValue}" />
    public class StoredPropertyStep<TValue> : IPropertyStep<TValue>, IStoredProperty<TValue>
    {
        /// <summary>
        ///     Gets or sets the stored <typeparamref name="TValue" />.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StoredPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public StoredPropertyStep(TValue initialValue = default)
        {
            Value = initialValue;
        }

        /// <summary>
        ///     Called when a value is read from the property.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        TValue IPropertyStep<TValue>.Get(IMockInfo mockInfo)
        {
            return Value;
        }

        /// <summary>
        ///     Called when a value is written to the property.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        void IPropertyStep<TValue>.Set(IMockInfo mockInfo, TValue value)
        {
            Value = value;
        }
    }
}
