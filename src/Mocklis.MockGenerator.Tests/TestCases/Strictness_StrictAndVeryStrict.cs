using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int Property { get; set; }
        string this[int i] { get; set; }
        string MyMethod(string param);
        event EventHandler MyEvent;
    }

    [MocklisClass(Strict = true, VeryStrict = true)]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
