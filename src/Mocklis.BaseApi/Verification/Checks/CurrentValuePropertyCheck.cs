// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentValuePropertyCheck.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Checks
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Check that verifies that the current value in a property store is the expected value.
    ///     Implements the <see cref="IVerifiable" /> interface.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="IVerifiable" />
    public class CurrentValuePropertyCheck<TValue> : IVerifiable
    {
        private readonly string? _name;
        private readonly IStoredProperty<TValue> _property;
        private readonly TValue _expectedValue;
        private readonly IEqualityComparer<TValue> _comparer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CurrentValuePropertyCheck{TValue}" /> class.
        /// </summary>
        /// <param name="property">The property store to check.</param>
        /// <param name="name">A name that can be used to identify the check in its verification group.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="comparer">Optional parameter with a comparer used to verify that the values are equal.</param>
        public CurrentValuePropertyCheck(IStoredProperty<TValue> property, string? name, TValue expectedValue,
            IEqualityComparer<TValue>? comparer = null)
        {
            _name = name;
            _property = property ?? throw new ArgumentNullException(nameof(property));
            _expectedValue = expectedValue;
            _comparer = comparer ?? EqualityComparer<TValue>.Default;
        }

        /// <summary>
        ///     Checks that the current and expected values match and returns an <see cref="VerificationResult" />.
        /// </summary>
        /// <param name="provider">
        ///     An object that supplies culture-specific formatting information. Defaults to the current culture.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable{VerificationResult}" /> with information about the verifications and whether they
        ///     were successful.
        /// </returns>
        public IEnumerable<VerificationResult> Verify(IFormatProvider? provider = null)
        {
            provider ??= CultureInfo.CurrentCulture;

            string prefix = string.IsNullOrEmpty(_name) ? "Value check" : $"Value check '{_name}'";

            TValue currentValue = _property.Value;
            string? expectedValueString = Convert.ToString(_expectedValue, provider);
            string? currentValueString = Convert.ToString(currentValue, provider);
            yield return new VerificationResult(
                $"{prefix}: Expected {expectedValueString.QuotedOrNull()}; Current Value is {currentValueString.QuotedOrNull()}",
                _comparer.Equals(_expectedValue, currentValue));
        }
    }
}
