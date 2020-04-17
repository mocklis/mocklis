using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<TOuter>
    {
        void Test<TInner>(TInner parameter)
            where TInner: TOuter;
    }

    [MocklisClass]
    public class TestClass<T> : ITestClass<T>
    {
    }
}
