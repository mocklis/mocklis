// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using Mocklis.Core;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockMethodStep()
        {
            Call = new FuncMethodMock<(IMockInfo mockInfo, TParam param), TResult>(this, "MockMethodStep", "IMethodStep", "Call", "Call",
                Strictness.Lenient);
        }

        public FuncMethodMock<(IMockInfo mockInfo, TParam param), TResult> Call { get; }

        TResult IMethodStep<TParam, TResult>.Call(IMockInfo mockInfo, TParam param) => Call.Call((mockInfo, param));
    }
}
