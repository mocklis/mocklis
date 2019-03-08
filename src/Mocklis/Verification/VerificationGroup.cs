// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationGroup.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    #endregion

    /// <summary>
    ///     Class that represents a group of verifications.
    ///     Implements the <see cref="IVerifiable" /> interface.
    /// </summary>
    /// <seealso cref="IVerifiable" />
    public sealed class VerificationGroup : IVerifiable
    {
        private readonly List<IVerifiable> _verifiables = new List<IVerifiable>();

        /// <summary>
        ///     Gets the name of the verification group.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VerificationGroup" /> class.
        /// </summary>
        /// <param name="name">Optional parameter with the name of the verification group.</param>
        public VerificationGroup(string name = null)
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
        private VerificationResult VerifyGroup(IFormatProvider provider)
        {
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
        IEnumerable<VerificationResult> IVerifiable.Verify(IFormatProvider provider)
        {
            provider = provider ?? CultureInfo.CurrentCulture;

            yield return VerifyGroup(provider);
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
        public void Assert(bool includeSuccessfulVerifications = false, IFormatProvider provider = null)
        {
            provider = provider ?? CultureInfo.CurrentCulture;

            VerificationResult result = VerifyGroup(provider);
            if (!result.Success)
            {
                var message = "Verification failed." + Environment.NewLine + Environment.NewLine + result.ToString(includeSuccessfulVerifications);
                throw new VerificationFailedException(result, message);
            }
        }
    }
}
