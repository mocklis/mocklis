using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef();
        ref readonly int ReturnsByRefReadonly();
        ref readonly int ReturnsMoreStuffByRef(out int blah);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
