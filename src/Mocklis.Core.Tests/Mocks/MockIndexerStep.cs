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
            Get = new FuncMethodMock<(IMockInfo mockInfo, TKey key), TValue>(this, "MockIndexerStep", "IIndexerStep", "Get", "Get");
            Set = new ActionMethodMock<(IMockInfo mockInfo, TKey key, TValue value)>(this, "MockIndexerStep", "IIndexerStep", "Set", "Set");
        }

        public FuncMethodMock<(IMockInfo mockInfo, TKey key), TValue> Get { get; }

        TValue IIndexerStep<TKey, TValue>.Get(IMockInfo mockInfo, TKey key) => Get.Call((mockInfo, key));

        public ActionMethodMock<(IMockInfo mockInfo, TKey key, TValue value)> Set { get; }

        void IIndexerStep<TKey, TValue>.Set(IMockInfo mockInfo, TKey key, TValue value) => Set.Call((mockInfo, key, value));
    }
}
