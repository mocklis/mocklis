// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMembers.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Mocks
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Tests.Interfaces;

    #endregion

    [MocklisClass]
    public class MockMembers : IEvents, IMethods
    {
        public MockMembers()
        {
            MyEvent = new EventMock<EventHandler>(this, "MockMembers", "IEvents", "MyEvent", "MyEvent");
            DoStuff = new FuncMethodMock<int, int>(this, "MockMembers", "IMethods", "DoStuff", "DoStuff");
        }

        public EventMock<EventHandler> MyEvent { get; }

        event EventHandler IEvents.MyEvent
        {
            add => MyEvent.Add(value);
            remove => MyEvent.Remove(value);
        }

        public FuncMethodMock<int, int> DoStuff { get; }

        int IMethods.DoStuff(int parameter) => DoStuff.Call(parameter);
    }
}
