using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int Test { get; }
        int Test2 { get; }
    }

    public class BaseClass
    {
        public BaseClass(int Test)
        {
        }
    }

    [MocklisClass]
    public class TestClass : BaseClass, ITestClass
    {
    }
}
