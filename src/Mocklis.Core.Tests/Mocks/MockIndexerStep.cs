// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Mocks
{
    [MocklisClass]
    public class MockIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        public MockIndexerStep()
        {
            Get = new FuncMethodMock<(MemberMock memberMock, TKey key), TValue>(this, "MockIndexerStep", "IIndexerStep", "Get", "Get");
            Set = new ActionMethodMock<(MemberMock memberMock, TKey key, TValue value)>(this, "MockIndexerStep", "IIndexerStep", "Set", "Set");
        }

        public FuncMethodMock<(MemberMock memberMock, TKey key), TValue> Get { get; }

        TValue IIndexerStep<TKey, TValue>.Get(MemberMock memberMock, TKey key) => Get.Call((memberMock, key));

        public ActionMethodMock<(MemberMock memberMock, TKey key, TValue value)> Set { get; }

        void IIndexerStep<TKey, TValue>.Set(MemberMock memberMock, TKey key, TValue value) => Set.Call((memberMock, key, value));
    }
}
