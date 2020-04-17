using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int i] { get; set; }
        int this[string s] { set; }
        int this[char c] { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
