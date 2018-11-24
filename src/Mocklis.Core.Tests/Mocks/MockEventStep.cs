// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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
            Add = new ActionMethodMock<(MemberMock memberMock, THandler value)>(this, "MockEventStep", "IEventStep", "Add", "Add");
            Remove = new ActionMethodMock<(MemberMock memberMock, THandler value)>(this, "MockEventStep", "IEventStep", "Remove", "Remove");
        }

        public ActionMethodMock<(MemberMock memberMock, THandler value)> Add { get; }

        void IEventStep<THandler>.Add(MemberMock memberMock, THandler value) => Add.Call((memberMock, value));

        public ActionMethodMock<(MemberMock memberMock, THandler value)> Remove { get; }

        void IEventStep<THandler>.Remove(MemberMock memberMock, THandler value) => Remove.Call((memberMock, value));
    }
}
