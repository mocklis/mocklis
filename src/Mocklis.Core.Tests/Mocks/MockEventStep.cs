// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Mocks
{
    #region Using Directives

    using System;

    #endregion

    [MocklisClass]
    public class MockEventStep<THandler> : IEventStep<THandler> where THandler : Delegate
    {
        public MockEventStep()
        {
            Add = new ActionMethodMock<(IMockInfo mockInfo, THandler value)>(this, "MockEventStep", "IEventStep", "Add", "Add", Strictness.Lenient);
            Remove = new ActionMethodMock<(IMockInfo mockInfo, THandler value)>(this, "MockEventStep", "IEventStep", "Remove", "Remove",
                Strictness.Lenient);
        }

        public ActionMethodMock<(IMockInfo mockInfo, THandler value)> Add { get; }

        void IEventStep<THandler>.Add(IMockInfo mockInfo, THandler value) => Add.Call((mockInfo, value));

        public ActionMethodMock<(IMockInfo mockInfo, THandler value)> Remove { get; }

        void IEventStep<THandler>.Remove(IMockInfo mockInfo, THandler value) => Remove.Call((mockInfo, value));
    }
}
