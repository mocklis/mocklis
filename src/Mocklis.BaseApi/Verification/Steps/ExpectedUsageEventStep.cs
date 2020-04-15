// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsageEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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
    ///     Event step that counts the number of times event handlers have been added or removed, and can verify
    ///     these against given expected number of adds and removes. This class cannot be inherited.
    ///     Inherits from the <see cref="EventStepWithNext{THandler}" /> class.
    ///     Implements the <see cref="IVerifiable" /> interface.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="EventStepWithNext{THandler}" />
    /// <seealso cref="IVerifiable" />
    public sealed class ExpectedUsageEventStep<THandler> : EventStepWithNext<THandler>, IVerifiable where THandler : Delegate
    {
        private readonly string? _name;
        private readonly int? _expectedNumberOfAdds;
        private int _currentNumberOfAdds;
        private readonly int? _expectedNumberOfRemoves;
        private int _currentNumberOfRemoves;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpectedUsageEventStep{THandler}" /> class.
        /// </summary>
        /// <param name="name">The name of the verification.</param>
        /// <param name="expectedNumberOfAdds">The expected number of event handler adds.</param>
        /// <param name="expectedNumberOfRemoves">The expected number of event handler removes.</param>
        public ExpectedUsageEventStep(string? name, int? expectedNumberOfAdds,
            int? expectedNumberOfRemoves)
        {
            if (expectedNumberOfAdds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedNumberOfAdds),
                    "Expected number of adds must not be negative. Pass 'null' to remove check.");
            }

            if (expectedNumberOfRemoves < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedNumberOfRemoves),
                    "Expected number of removes must not be negative. Pass 'null' to remove check.");
            }

            _name = name;
            _expectedNumberOfAdds = expectedNumberOfAdds;
            _expectedNumberOfRemoves = expectedNumberOfRemoves;
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        ///     Increases a counter that keeps track of the number of times event handlers have been added.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public override void Add(IMockInfo mockInfo, THandler? value)
        {
            Interlocked.Increment(ref _currentNumberOfAdds);
            base.Add(mockInfo, value);
        }

        /// <summary>
        ///     Called when an event handler is being removed from the mocked event.
        ///     Increases a counter that keeps track of the number of times event handlers have been removed.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public override void Remove(IMockInfo mockInfo, THandler? value)
        {
            Interlocked.Increment(ref _currentNumberOfRemoves);
            base.Remove(mockInfo, value);
        }

        /// <summary>
        ///     Verifies that the expected number of event handlers have been added and removed, and returns the result of the
        ///     verifications.
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

            if (_expectedNumberOfAdds is int expectedAdds)
            {
                string expectedAddsString = expectedAdds.ToString();
                string currentAddsString = _currentNumberOfAdds.ToString();
                yield return new VerificationResult($"{prefix}: Expected {expectedAddsString} add(s); received {currentAddsString} add(s).",
                    expectedAdds == _currentNumberOfAdds);
            }

            if (_expectedNumberOfRemoves is int expectedRemoves)
            {
                string expectedRemovesString = expectedRemoves.ToString();
                string currentRemovesString = _currentNumberOfRemoves.ToString();

                yield return new VerificationResult(
                    $"{prefix}: Expected {expectedRemovesString} remove(s); received {currentRemovesString} remove(s).",
                    expectedRemoves == _currentNumberOfRemoves);
            }
        }
    }
}
