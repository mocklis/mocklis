using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public ref struct RefStruct
    {
        public int Test { get; set;}
    }

    public interface ITestClass
    {
        RefStruct ReturnsRefStruct();
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
