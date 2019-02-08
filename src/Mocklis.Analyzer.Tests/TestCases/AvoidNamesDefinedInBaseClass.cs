using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Test(int i);
        bool AnotherTest { get; set; }
    }

    public class BaseClass
    {
        event EventHandler Test;
        void AnotherTest<T>(Action<T> test)
        {
        }
    }

    [MocklisClass]
    public class TestClass : BaseClass, ITestClass
    {
    }
}
