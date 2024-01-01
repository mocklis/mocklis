// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoredProperty.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Interface that provides access to the value in a 'stored' property step.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    public interface IStoredProperty<out TValue>
    {
        /// <summary>
        ///     Gets the <typeparamref name="TValue" />.
        /// </summary>
        /// <returns>The <typeparamref name="TValue" /> to get.</returns>
        TValue Value { get; }
    }
}
