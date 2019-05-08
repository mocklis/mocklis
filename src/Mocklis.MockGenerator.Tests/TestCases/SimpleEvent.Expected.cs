using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        event EventHandler MyEvent;
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            MyEvent = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "MyEvent", "MyEvent", Strictness.Lenient);
        }

        public EventMock<EventHandler> MyEvent { get; }

        event EventHandler ITestClass.MyEvent { add => MyEvent.Add(value); remove => MyEvent.Remove(value); }
    }
}
