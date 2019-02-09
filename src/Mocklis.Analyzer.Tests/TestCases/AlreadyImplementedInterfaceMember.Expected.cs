using System;
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
        public TestClass()
        {
            Test = new FuncMethodMock<int, string>(this, "TestClass", "ITestClass", "Test", "Test");
        }

        public FuncMethodMock<int, string> Test { get; }

        string ITestClass.Test(int i) => Test.Call(i);
    }
}