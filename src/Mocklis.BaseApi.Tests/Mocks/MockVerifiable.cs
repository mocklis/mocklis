// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockVerifiable.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Verification;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockVerifiable : IVerifiable
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockVerifiable()
        {
            Verify = new FuncMethodMock<IFormatProvider?, IEnumerable<VerificationResult>>(this, "MockVerifiable", "IVerifiable", "Verify", "Verify",
                Strictness.Lenient);
        }

        public FuncMethodMock<IFormatProvider?, IEnumerable<VerificationResult>> Verify { get; }

        IEnumerable<VerificationResult> IVerifiable.Verify(IFormatProvider? provider) => Verify.Call(provider);
    }
}
