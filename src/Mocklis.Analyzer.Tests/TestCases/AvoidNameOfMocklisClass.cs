using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string TestClass(int i);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
