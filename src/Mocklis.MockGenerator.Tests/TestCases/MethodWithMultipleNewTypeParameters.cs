using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Combine<T1, T2>(T1 param1, T2 param2);
        string Combine<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
        string Combine<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4);
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
