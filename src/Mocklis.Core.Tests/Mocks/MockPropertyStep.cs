// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Mocks
{
    [MocklisClass]
    public class MockPropertyStep<TValue> : IPropertyStep<TValue>
    {
        public MockPropertyStep()
        {
            Get = new FuncMethodMock<MemberMock, TValue>(this, "PropertyStep", "IPropertyStep", "Get", "Get");
            Set = new ActionMethodMock<(MemberMock memberMock, TValue value)>(this, "PropertyStep", "IPropertyStep", "Set", "Set");
        }

        public FuncMethodMock<MemberMock, TValue> Get { get; }

        TValue IPropertyStep<TValue>.Get(MemberMock memberMock) => Get.Call(memberMock);

        public ActionMethodMock<(MemberMock memberMock, TValue value)> Set { get; }

        void IPropertyStep<TValue>.Set(MemberMock memberMock, TValue value) => Set.Call((memberMock, value));
    }
}
