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
    public class MockMembers : IEvents, IMethods, IProperties, IIndexers
    {
        public MockMembers()
        {
            MyEvent = new EventMock<EventHandler>(this, "MockMembers", "IEvents", "MyEvent", "MyEvent");
            DoStuff = new FuncMethodMock<int, int>(this, "MockMembers", "IMethods", "DoStuff", "DoStuff");
            Name = new PropertyMock<string>(this, "MockMembers", "IProperties", "Name", "Name");
            Age = new PropertyMock<int>(this, "MockMembers", "IProperties", "Age", "Age");
            Item = new IndexerMock<int, string>(this, "MockMembers", "IIndexers", "this[]", "Item");
        }

        public EventMock<EventHandler> MyEvent { get; }

        event EventHandler IEvents.MyEvent
        {
            add => MyEvent.Add(value);
            remove => MyEvent.Remove(value);
        }

        public FuncMethodMock<int, int> DoStuff { get; }

        int IMethods.DoStuff(int parameter) => DoStuff.Call(parameter);

        public PropertyMock<string> Name { get; }

        string IProperties.Name
        {
            get => Name.Value;
            set => Name.Value = value;
        }

        public PropertyMock<int> Age { get; }

        int IProperties.Age
        {
            get => Age.Value;
            set => Age.Value = value;
        }

        public IndexerMock<int, string> Item { get; }

        string IIndexers.this[int index]
        {
            get => Item[index];
            set => Item[index] = value;
        }
    }
}
