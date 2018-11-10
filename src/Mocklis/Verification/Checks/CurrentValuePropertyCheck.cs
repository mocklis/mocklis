// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentValuePropertyCheck.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Checks
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using static System.FormattableString;

    #endregion

    public class CurrentValuePropertyCheck<TValue> : IVerifiable
    {
        private readonly string _name;
        private readonly IStoredProperty<TValue> _property;
        private readonly TValue _expectedValue;
        private readonly IEqualityComparer<TValue> _comparer;

        public CurrentValuePropertyCheck(IStoredProperty<TValue> property, string name, TValue expectedValue,
            IEqualityComparer<TValue> comparer = null)
        {
            _name = name;
            _property = property ?? throw new ArgumentNullException(nameof(property));
            _expectedValue = expectedValue;
            _comparer = comparer ?? EqualityComparer<TValue>.Default;
        }

        public IEnumerable<VerificationResult> Verify()
        {
            string prefix = string.IsNullOrEmpty(_name) ? "Value check" : Invariant($"Value check '{_name}'");
            TValue currentValue = _property.Value;
            yield return new VerificationResult(Invariant($"{prefix}: Expected '{_expectedValue}'; Current Value is '{currentValue}"),
                _comparer.Equals(_expectedValue, currentValue));
        }
    }
}
