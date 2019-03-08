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
    }
}
