using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Concat(string s1, string s2, __arglist);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
