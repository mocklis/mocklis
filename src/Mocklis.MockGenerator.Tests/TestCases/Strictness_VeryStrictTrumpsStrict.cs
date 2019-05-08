using System;
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

    [MocklisClass(Strict = false, VeryStrict = true)]
    public class TestClass : ITestClass
    {
    }
}
