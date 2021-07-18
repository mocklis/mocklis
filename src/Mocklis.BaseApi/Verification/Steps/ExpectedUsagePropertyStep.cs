// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsagePropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Steps
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Property step that counts the number of times values have been read from and written to the property, and can
    ///     verify
    ///     these against given expected number of reads and writes. This class cannot be inherited.
    ///     Implements the <see cref="PropertyStepWithNext{TValue}" />
    ///     Implements the <see cref="IVerifiable" />
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    /// <seealso cref="IVerifiable" />
    public sealed class ExpectedUsagePropertyStep<TValue> : PropertyStepWithNext<TValue>, IVerifiable
    {
        private readonly string? _name;
        private readonly int? _expectedNumberOfGets;
        private int _currentNumberOfGets;
        private readonly int? _expectedNumberOfSets;
        private int _currentNumberOfSets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpectedUsagePropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="name">The name of the verification.</param>
        /// <param name="expectedNumberOfGets">The expected number of reads from the property.</param>
        /// <param name="expectedNumberOfSets">The expected number of writes to the property.</param>
        public ExpectedUsagePropertyStep(string? name, int? expectedNumberOfGets,
            int? expectedNumberOfSets)
        {
            if (expectedNumberOfGets < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedNumberOfGets),
                    "Expected number of gets must not be negative. Pass 'null' to remove check.");
            }

            if (expectedNumberOfSets < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedNumberOfSets),
                    "Expected number of sets must not be negative. Pass 'null' to remove check.");
            }

            _name = name;
            _expectedNumberOfGets = expectedNumberOfGets;
            _expectedNumberOfSets = expectedNumberOfSets;
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     Increases a counter that keeps track of the number of times values have been read.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            Interlocked.Increment(ref _currentNumberOfGets);
            return base.Get(mockInfo);
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     Increases a counter that keeps track of the number of times values have been written.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TValue value)
        {
            Interlocked.Increment(ref _currentNumberOfSets);
            base.Set(mockInfo, value);
        }

        /// <summary>
        ///     Verifies that the expected number of reads from and writes to the property have occurred and returns the result of
        ///     the verifications.
        /// </summary>
        /// <param name="provider">
        ///     An object that supplies culture-specific formatting information. Not used for this implementation since
        ///     we'd only be formatting non-negative <see cref="int" /> values.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable{VerificationResult}" /> with information about the verifications and whether they
        ///     were successful.
        /// </returns>
        public IEnumerable<VerificationResult> Verify(IFormatProvider? provider = null)
        {
            string prefix = string.IsNullOrEmpty(_name) ? "Usage Count" : $"Usage Count '{_name}'";

            if (_expectedNumberOfGets is int expectedGets)
            {
                string expectedGetsString = expectedGets.ToString();
                string currentGetsString = _currentNumberOfGets.ToString();

                yield return new VerificationResult($"{prefix}: Expected {expectedGetsString} get(s); received {currentGetsString} get(s).",
                    expectedGets == _currentNumberOfGets);
            }

            if (_expectedNumberOfSets is int expectedSets)
            {
                string expectedSetsString = expectedSets.ToString();
                string currentSetsString = _currentNumberOfSets.ToString();

                yield return new VerificationResult($"{prefix}: Expected {expectedSetsString} set(s); received {currentSetsString} set(s).",
                    expectedSets == _currentNumberOfSets);
            }
        }
    }
}
