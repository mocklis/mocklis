using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass : IDisposable
    {
        string Test(int i);
    }

    public class BaseClass : IDisposable
    {
        public void Dispose()
        {
        }
    }

    [MocklisClass]
    public class TestClass : BaseClass, ITestClass
    {
    }
}
