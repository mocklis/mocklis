using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int GetAndSet { get; set; }
        int SetOnly { set; }
        int GetOnly { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
