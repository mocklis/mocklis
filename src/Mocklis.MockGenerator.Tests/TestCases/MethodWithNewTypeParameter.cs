using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        T ReturnsNewType<T>();
        void UsesNewType<T>(T parameter);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
