using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        T2 Test<T1, T2>(T1 parameter)
            where T1: struct
            where T2 : class, IDisposable, new();
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
