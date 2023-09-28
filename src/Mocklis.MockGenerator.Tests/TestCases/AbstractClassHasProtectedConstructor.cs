using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int Test { get; }
    }

    [MocklisClass]
    public abstract [PARTIAL] class TestClass : ITestClass
    {
    }
}
