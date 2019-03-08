using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T>
    {
        T GetAndSet { get; set; }
    }

    [MocklisClass]
    public class TestClass<T> : ITestClass<T>
    {
    }
}
