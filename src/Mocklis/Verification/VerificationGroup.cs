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
    using System.Linq;

    #endregion

    /// <summary>
    ///     Class that represents a group of verifications.
    ///     Implements the <see cref="Mocklis.Verification.IVerifiable" /> interface.
    /// </summary>
    /// <seealso cref="Mocklis.Verification.IVerifiable" />
    public class VerificationGroup : IVerifiable
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
        /// <returns>A single <see cref="VerificationResult" /> representing the success of the entire group.</returns>
        public VerificationResult Verify()
        {
            string description = string.IsNullOrEmpty(Name) ? "Verification Group:" : $"Verification Group '{Name}':";
            return new VerificationResult(description, _verifiables.SelectMany(a => a.Verify()));
        }

        /// <summary>
        ///     Verifies a set of conditions and returns the result of the verifications.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable{VerificationResult}" /> with information about the verifications and whether they
        ///     were successful.
        /// </returns>
        IEnumerable<VerificationResult> IVerifiable.Verify()
        {
            yield return Verify();
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
        /// <param name="includeSuccessfulVerifications">Whether to include successful verifications in the exception if thrown.</param>
        public void Assert(bool includeSuccessfulVerifications = false)
        {
            VerificationResult result = Verify();
            if (!result.Success)
            {
                var message = "Verification failed." + Environment.NewLine + Environment.NewLine + result.ToString(includeSuccessfulVerifications);
                throw new VerificationFailedException(result, message);
            }
        }
    }
}
