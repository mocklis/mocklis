// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationResultExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using Mocklis.Verification;
    using Xunit;

    #endregion

    public static class VerificationResultExtensions
    {
        public static void AssertEquals(this VerificationResult actual, VerificationResult expected)
        {
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Success, actual.Success);
            Assert.Equal(expected.SubResults.Count, actual.SubResults.Count);

            for (var i = 0; i < expected.SubResults.Count; i++)
            {
                AssertEquals(expected.SubResults[i], actual.SubResults[i]);
            }
        }
    }
}
