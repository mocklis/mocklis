// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCalculator.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using Mocklis.Core;
    using Mocklis.Interfaces;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockCalculator : ICalculator
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockCalculator()
        {
            Calculate = new FuncMethodMock<int, int>(this, "MockCalculator", "ICalculator", "Calculate", "Calculate", Strictness.Lenient);
        }

        public FuncMethodMock<int, int> Calculate { get; }

        int ICalculator.Calculate(int data) => Calculate.Call(data);
    }
}
