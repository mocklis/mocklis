// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using Mocklis.Core;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockPropertyStep<TValue> : IPropertyStep<TValue>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockPropertyStep()
        {
            Get = new FuncMethodMock<IMockInfo, TValue>(this, "MockPropertyStep", "IPropertyStep", "Get", "Get", Strictness.Lenient);
            Set = new ActionMethodMock<(IMockInfo mockInfo, TValue value)>(this, "MockPropertyStep", "IPropertyStep", "Set", "Set", Strictness.Lenient);
        }

        public FuncMethodMock<IMockInfo, TValue> Get { get; }

        TValue IPropertyStep<TValue>.Get(IMockInfo mockInfo) => Get.Call(mockInfo);

        public ActionMethodMock<(IMockInfo mockInfo, TValue value)> Set { get; }

        void IPropertyStep<TValue>.Set(IMockInfo mockInfo, TValue value) => Set.Call((mockInfo, value));
    }
}
