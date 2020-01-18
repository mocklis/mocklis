// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Mocks
{
    [MocklisClass]
    public class MockIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockIndexerStep()
        {
            Get = new FuncMethodMock<(IMockInfo mockInfo, TKey key), TValue>(this, "MockIndexerStep", "IIndexerStep", "Get", "Get", Strictness.Lenient);
            Set = new ActionMethodMock<(IMockInfo mockInfo, TKey key, TValue value)>(this, "MockIndexerStep", "IIndexerStep", "Set", "Set", Strictness.Lenient);
        }

        public FuncMethodMock<(IMockInfo mockInfo, TKey key), TValue> Get { get; }

        TValue IIndexerStep<TKey, TValue>.Get(IMockInfo mockInfo, TKey key) => Get.Call((mockInfo, key));

        public ActionMethodMock<(IMockInfo mockInfo, TKey key, TValue value)> Set { get; }

        void IIndexerStep<TKey, TValue>.Set(IMockInfo mockInfo, TKey key, TValue value) => Set.Call((mockInfo, key, value));
    }
}
