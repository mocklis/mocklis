// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMembers.cs">
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
    using Mocklis.Interfaces;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockMembers : IMembers
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockMembers()
        {
            MyEvent = new EventMock<EventHandler>(this, "MockMembers", "IMembers", "MyEvent", "MyEvent", Strictness.Lenient);
            Item = new IndexerMock<int, string>(this, "MockMembers", "IMembers", "this[]", "Item", Strictness.Lenient);
            DoStuff = new ActionMethodMock(this, "MockMembers", "IMembers", "DoStuff", "DoStuff", Strictness.Lenient);
            Calculate = new FuncMethodMock<(int value1, int value2), int>(this, "MockMembers", "IMembers", "Calculate", "Calculate", Strictness.Lenient);
            StringProperty = new PropertyMock<string>(this, "MockMembers", "IMembers", "StringProperty", "StringProperty", Strictness.Lenient);
        }

        public EventMock<EventHandler> MyEvent { get; }

        event EventHandler? IMembers.MyEvent { add => MyEvent.Add(value); remove => MyEvent.Remove(value); }

        public IndexerMock<int, string> Item { get; }

        string IMembers.this[int index] { get => Item[index]; set => Item[index] = value; }

        public ActionMethodMock DoStuff { get; }

        void IMembers.DoStuff() => DoStuff.Call();

        public FuncMethodMock<(int value1, int value2), int> Calculate { get; }

        int IMembers.Calculate(int value1, int value2) => Calculate.Call((value1, value2));

        public PropertyMock<string> StringProperty { get; }
        string IMembers.StringProperty { get => StringProperty.Value; set => StringProperty.Value = value; }
    }
}
