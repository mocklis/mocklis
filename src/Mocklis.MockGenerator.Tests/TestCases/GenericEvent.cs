using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T> where T : EventArgs
    {
        event EventHandler<T> MyEvent;
    }

    [MocklisClass]
    public [PARTIAL] class TestClass<T> : ITestClass<T> where T : EventArgs
    {
    }
}
