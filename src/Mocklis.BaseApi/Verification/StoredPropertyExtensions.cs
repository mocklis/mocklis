// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredPropertyExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Verification.Checks;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding checks to an existing property store.
    /// </summary>
    public static class StoredPropertyExtensions
    {
        /// <summary>
        ///     Checks the current values in the property store. Adds the check to the verification group provided.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="property">The <see cref="IStoredProperty{TValue}" /> whose value to check.</param>
        /// <param name="collector">The verification group to which this check is added.</param>
        /// <param name="name">A name that can be used to identify the check in its verification group.</param>
        /// <param name="expectedValue">The expected value. </param>
        /// <param name="comparer">Optional parameter with a comparer used to verify that the values are equal.</param>
        /// <returns>The <see cref="IStoredProperty{TValue}" /> instance that can be used to add further checks.</returns>
        public static IStoredProperty<TValue> CurrentValueCheck<TValue>(this IStoredProperty<TValue> property, VerificationGroup collector,
            string? name, TValue expectedValue, IEqualityComparer<TValue>? comparer = null)
        {
            collector.Add(new CurrentValuePropertyCheck<TValue>(property, name, expectedValue, comparer));
            return property;
        }
    }
}
