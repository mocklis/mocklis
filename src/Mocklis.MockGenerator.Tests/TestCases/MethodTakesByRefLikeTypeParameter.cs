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
        void TakesRefStructParameter(RefStruct parameter);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
