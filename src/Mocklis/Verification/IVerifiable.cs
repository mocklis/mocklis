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

    public interface IVerifiable
    {
        IEnumerable<VerificationResult> Verify();
    }
}
