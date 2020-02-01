// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCalculator.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Experimental.Tests.Mocks
{
    #region Using Directives

    using Mocklis.Core;
    using Mocklis.Experimental.Tests.Interfaces;

    #endregion

    [MocklisClass]
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
