using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int i, string s] { get; set; }
        int this[string s, string s2] { set; }
        int this[char c, int i, string s] { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
