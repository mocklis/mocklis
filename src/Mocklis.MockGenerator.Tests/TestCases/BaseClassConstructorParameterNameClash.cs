using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int Test { get; }
        int Test2 { get; }
    }

    public class BaseClass
    {
        public BaseClass(int Test)
        {
        }
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : BaseClass, ITestClass
    {
    }
}
