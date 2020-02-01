using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T>
    {
        void Test<U>(U parameter) where U : T;
    }

    [MocklisClass]
    public class TestClass : ITestClass<int>
    {
    }
}
