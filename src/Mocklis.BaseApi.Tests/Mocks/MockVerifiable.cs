// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockVerifiable.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using Mocklis.Core;
    using Mocklis.Verification;

    #endregion

    [MocklisClass, GeneratedCode("Mocklis", "1.2.0")]
    public class MockVerifiable : IVerifiable
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockVerifiable()
        {
            Verify = new FuncMethodMock<System.IFormatProvider?, System.Collections.Generic.IEnumerable<VerificationResult>>(this, "MockVerifiable", "IVerifiable", "Verify", "Verify", Strictness.Lenient);
        }

        public FuncMethodMock<System.IFormatProvider?, System.Collections.Generic.IEnumerable<VerificationResult>> Verify { get; }

        System.Collections.Generic.IEnumerable<VerificationResult> IVerifiable.Verify(System.IFormatProvider? provider) => Verify.Call(provider);
    }
}
