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
        public TestClass()
        {
            MyEvent = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "MyEvent", "MyEvent");
        }

        public EventMock<EventHandler> MyEvent { get; }

        event EventHandler ITestClass.MyEvent { add => MyEvent.Add(value); remove => MyEvent.Remove(value); }
    }
}
