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
        public TestClass()
        {
            GetAndSet = new PropertyMock<T>(this, "TestClass", "ITestClass", "GetAndSet", "GetAndSet");
        }

        public PropertyMock<T> GetAndSet { get; }
        T ITestClass<T>.GetAndSet { get => GetAndSet.Value; set => GetAndSet.Value = value; }
    }
}
