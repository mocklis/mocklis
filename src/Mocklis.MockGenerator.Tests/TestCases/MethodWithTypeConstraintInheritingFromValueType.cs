using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T>
    {
        void Test<U>(U parameter) where U : T;
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass<int>
    {
    }
}
