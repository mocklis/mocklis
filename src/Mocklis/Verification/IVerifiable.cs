// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVerifiable.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Interface that represents something that can verify a set of conditions and return the results of the
    ///     verifications.
    /// </summary>
    public interface IVerifiable
    {
        /// <summary>
        ///     Verifies a set of conditions and returns the result of the verifications.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerable{VerificationResult}" /> with information about the verifications and whether they
        ///     were successful.
        /// </returns>
        IEnumerable<VerificationResult> Verify();
    }
}
