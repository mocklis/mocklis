using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Concat(string arglist, __arglist);
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
