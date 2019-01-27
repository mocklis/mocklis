using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int TestClass { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
