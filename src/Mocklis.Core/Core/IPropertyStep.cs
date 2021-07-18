// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Interface that models things that can happen with a mocked property
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    public interface IPropertyStep<TValue>
    {
        /// <summary>
        ///     Called when a value is read from the property.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        TValue Get(IMockInfo mockInfo);

        /// <summary>
        ///     Called when a value is written to the property.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        void Set(IMockInfo mockInfo, TValue value);
    }
}
