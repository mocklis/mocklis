// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationGroup.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    #endregion

    /// <summary>
    ///     Class that represents a group of verifications.
    ///     Implements the <see cref="IVerifiable" /> interface.
    ///     Implements the <see cref="IEnumerable{T}" /> interface.
    /// </summary>
    /// <seealso cref="IVerifiable" />
    /// <seealso cref="IEnumerable{T}" />
    public sealed class VerificationGroup : IVerifiable, IEnumerable<IVerifiable>
    {
        private readonly List<IVerifiable> _verifiables = new List<IVerifiable>();

        /// <summary>
        ///     Gets the name of the verification group.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VerificationGroup" /> class.
        /// </summary>
        /// <param name="name">Optional parameter with the name of the verification group.</param>
        public VerificationGroup(string? name = null)
        {
            Name = name;
        }

        /// <summary>
        ///     Verifies all the verifications in this group and returns the result.
        /// </summary>
        /// <param name="provider">
        ///     An object that supplies culture-specific formatting information. Defaults to the current culture.
        /// </param>
        /// <returns>A single <see cref="VerificationResult" /> representing the success of the entire group.</returns>
        private VerificationResult VerifyGroup(IFormatProvider? provider)
        {
            provider ??= CultureInfo.CurrentCulture;

            string description = string.IsNullOrEmpty(Name) ? "Verification Group:" : $"Verification Group '{Name}':";
            return new VerificationResult(description, _verifiables.SelectMany(a => a.Verify(provider)));
        }

        /// <summary>
        ///     Verifies a set of conditions and returns the result of the verifications.
        /// </summary>
        /// <param name="provider">
        ///     An object that supplies culture-specific formatting information. Defaults to the current culture.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable{VerificationResult}" /> with information about the verifications and whether they
        ///     were successful.
        /// </returns>
        IEnumerable<VerificationResult> IVerifiable.Verify(IFormatProvider? provider)
        {
            // Return the single value as an IEnumerable. Note that we want to verify right away, which is why
            // 'yield return' is not used.
            return Enumerable.Repeat(VerifyGroup(provider), 1);
        }

        /// <summary>
        ///     Adds a <see cref="IVerifiable" /> instance to the group.
        /// </summary>
        /// <param name="verifiable">The new <see cref="IVerifiable" /> instance we want to add.</param>
        public void Add(IVerifiable verifiable)
        {
            _verifiables.Add(verifiable);
        }

        /// <summary>
        ///     Asserts that the verifications in this group are all successful, throws a
        ///     <see cref="VerificationFailedException" /> otherwise.
        /// </summary>
        /// <param name="provider">
        ///     An object that supplies culture-specific formatting information. Defaults to the current culture.
        /// </param>
        /// <param name="includeSuccessfulVerifications">Whether to include successful verifications in the exception if thrown.</param>
        public void Assert(bool includeSuccessfulVerifications = false, IFormatProvider? provider = null)
        {
            VerificationResult result = VerifyGroup(provider);

            if (!result.Success)
            {
                var message = "Verification failed." + Environment.NewLine + Environment.NewLine + result.ToString(includeSuccessfulVerifications);
                throw new VerificationFailedException(result, message);
            }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the verifications in the group.
        /// </summary>
        public IEnumerator<IVerifiable> GetEnumerator() => _verifiables.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through the verifications in the group.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => _verifiables.GetEnumerator();
    }
}
