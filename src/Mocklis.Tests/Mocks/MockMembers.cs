// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMembers.cs">
//   SPDX-License-Identifier: MIT
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
            SimpleAction = new ActionMethodMock(this, "MockMembers", "IMethods", "SimpleAction", "SimpleAction");
            ActionWithParameter = new ActionMethodMock<int>(this, "MockMembers", "IMethods", "ActionWithParameter", "ActionWithParameter");
            SimpleFunc = new FuncMethodMock<int>(this, "MockMembers", "IMethods", "SimpleFunc", "SimpleFunc");
            FuncWithParameter = new FuncMethodMock<int, int>(this, "MockMembers", "IMethods", "FuncWithParameter", "FuncWithParameter");
            StringProperty = new PropertyMock<string>(this, "MockMembers", "IProperties", "StringProperty", "StringProperty");
            IntProperty = new PropertyMock<int>(this, "MockMembers", "IProperties", "IntProperty", "IntProperty");
            BoolProperty = new PropertyMock<bool>(this, "MockMembers", "IProperties", "BoolProperty", "BoolProperty");
            Item = new IndexerMock<int, string>(this, "MockMembers", "IIndexers", "this[]", "Item");
        }

        public EventMock<EventHandler> MyEvent { get; }

        event EventHandler IEvents.MyEvent
        {
            add => MyEvent.Add(value);
            remove => MyEvent.Remove(value);
        }

        public ActionMethodMock SimpleAction { get; }

        void IMethods.SimpleAction() => SimpleAction.Call();

        public ActionMethodMock<int> ActionWithParameter { get; }

        void IMethods.ActionWithParameter(int i) => ActionWithParameter.Call(i);

        public FuncMethodMock<int> SimpleFunc { get; }

        int IMethods.SimpleFunc() => SimpleFunc.Call();

        public FuncMethodMock<int, int> FuncWithParameter { get; }

        int IMethods.FuncWithParameter(int i) => FuncWithParameter.Call(i);

        public PropertyMock<string> StringProperty { get; }

        string IProperties.StringProperty
        {
            get => StringProperty.Value;
            set => StringProperty.Value = value;
        }

        public PropertyMock<int> IntProperty { get; }

        int IProperties.IntProperty
        {
            get => IntProperty.Value;
            set => IntProperty.Value = value;
        }

        public PropertyMock<bool> BoolProperty { get; }

        bool IProperties.BoolProperty
        {
            get => BoolProperty.Value;
            set => BoolProperty.Value = value;
        }

        public IndexerMock<int, string> Item { get; }

        string IIndexers.this[int index]
        {
            get => Item[index];
            set => Item[index] = value;
        }
    }
}
