using System;
using System.CodeDom.Compiler;
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
    }
}
