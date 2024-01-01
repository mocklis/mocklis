// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System;
    using System.CodeDom.Compiler;
    using Mocklis.Core;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockEventStep<THandler> : IEventStep<THandler> where THandler : Delegate
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockEventStep()
        {
            Add = new ActionMethodMock<(IMockInfo mockInfo, THandler? value)>(this, "MockEventStep", "IEventStep", "Add", "Add", Strictness.Lenient);
            Remove = new ActionMethodMock<(IMockInfo mockInfo, THandler? value)>(this, "MockEventStep", "IEventStep", "Remove", "Remove",
                Strictness.Lenient);
        }

        public ActionMethodMock<(IMockInfo mockInfo, THandler? value)> Add { get; }

        void IEventStep<THandler>.Add(IMockInfo mockInfo, THandler? value) => Add.Call((mockInfo, value));

        public ActionMethodMock<(IMockInfo mockInfo, THandler? value)> Remove { get; }

        void IEventStep<THandler>.Remove(IMockInfo mockInfo, THandler? value) => Remove.Call((mockInfo, value));
    }
}
