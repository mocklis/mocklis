using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef { get; }
        ref readonly int ReturnsByRefReadonly { get; }
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
