using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int TestClass { get; set; }
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
