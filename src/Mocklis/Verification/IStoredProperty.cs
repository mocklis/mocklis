// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoredProperty.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    /// <summary>
    ///     Interface that provides access to the value in a 'stored' property step.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    public interface IStoredProperty<TValue>
    {
        /// <summary>
        ///     Gets or sets the <typeparamref name="TValue" />.
        /// </summary>
        /// <returns>The <typeparamref name="TValue" /> to get or set.</returns>
        TValue Value { get; set; }
    }
}
