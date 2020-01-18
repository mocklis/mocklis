#nullable enable
using System;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass
    {
        event EventHandler NonNullableEvent;
        event EventHandler? NullableEvent;
    }

    [MocklisClass]
    public abstract class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected TestClass()
        {
            NonNullableEvent = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "NonNullableEvent", "NonNullableEvent", Strictness.Lenient);
            NullableEvent = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "NullableEvent", "NullableEvent", Strictness.Lenient);
        }

        public EventMock<EventHandler> NonNullableEvent { get; }

        event EventHandler ITestClass.NonNullableEvent { add => NonNullableEvent.Add(value); remove => NonNullableEvent.Remove(value); }

        public EventMock<EventHandler> NullableEvent { get; }

        event EventHandler? ITestClass.NullableEvent { add => NullableEvent.Add(value); remove => NullableEvent.Remove(value); }
    }
}
