using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<TOuter>
    {
        void Test<T>(TOuter outer, T parameter);
        void TestWithConstraint<T>(TOuter outer, T parameter) where T : TOuter;
    }

    [MocklisClass]
    public class TestClass<T> : ITestClass<T>
    {
    }
}
