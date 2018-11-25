// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Mocks
{
    [MocklisClass]
    public class MockMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        public MockMethodStep()
        {
            Call = new FuncMethodMock<(IMockInfo mockInfo, TParam param), TResult>(this, "MockMethodStep", "IMethodStep", "Call", "Call");
        }

        public FuncMethodMock<(IMockInfo mockInfo, TParam param), TResult> Call { get; }

        TResult IMethodStep<TParam, TResult>.Call(IMockInfo mockInfo, TParam param) => Call.Call((mockInfo, param));
    }
}
