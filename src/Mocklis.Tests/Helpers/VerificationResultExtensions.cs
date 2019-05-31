// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationResultExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Helpers
{
    #region Using Directives

    using Mocklis.Verification;
    using Xunit;

    #endregion

    public static class VerificationResultExtensions
    {
        public static void AssertEquals(this VerificationResult actual, VerificationResult expected)
        {
            bool ContainSameData(VerificationResult v1, VerificationResult v2)
            {
                if (v1.Description != v2.Description)
                {
                    return false;
                }

                if (v1.Success != v2.Success)
                {
                    return false;
                }

                if (v1.SubResults.Count != v2.SubResults.Count)
                {
                    return false;
                }

                for (var i = 0; i < v1.SubResults.Count; i++)
                {
                    if (!ContainSameData(v1.SubResults[i], v2.SubResults[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            Assert.True(ContainSameData(actual, expected));
        }
    }
}
