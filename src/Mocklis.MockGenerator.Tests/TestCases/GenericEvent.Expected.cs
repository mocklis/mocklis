using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T> where T : EventArgs
    {
        event EventHandler<T> MyEvent;
    }

    [MocklisClass]
    public class TestClass<T> : ITestClass<T> where T : EventArgs
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            MyEvent = new EventMock<EventHandler<T>>(this, "TestClass", "ITestClass", "MyEvent", "MyEvent");
        }

        public EventMock<EventHandler<T>> MyEvent { get; }

        event EventHandler<T> ITestClass<T>.MyEvent { add => MyEvent.Add(value); remove => MyEvent.Remove(value); }
    }
}
