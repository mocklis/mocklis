using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T, TResult>
    {
        TResult DoCalculation(T sourceData);
    }

    [MocklisClass]
    public class TestClass<T1, T2> : ITestClass<T1, T2>
    {
    }
}
