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
            Call = new FuncMethodMock<(MemberMock memberMock, TParam param), TResult>(this, "MockMethodStep", "IMethodStep", "Call", "Call");
        }

        public FuncMethodMock<(MemberMock memberMock, TParam param), TResult> Call { get; }

        TResult IMethodStep<TParam, TResult>.Call(MemberMock memberMock, TParam param) => Call.Call((memberMock, param));
    }
}
