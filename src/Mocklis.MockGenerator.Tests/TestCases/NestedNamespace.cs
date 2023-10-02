using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int GetOnly { get; }
    }

    namespace Test2
    {
        [MocklisClass]
        public [PARTIAL] class TestClass : ITestClass
        {
        }
    }
}
