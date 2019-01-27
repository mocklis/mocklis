using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int Test { get; }
    }

    [MocklisClass]
    public abstract class TestClass : ITestClass
    {
        protected TestClass()
        {
            Test = new PropertyMock<int>(this, "TestClass", "ITestClass", "Test", "Test");
        }

        public PropertyMock<int> Test { get; }

        int ITestClass.Test => Test.Value;
    }
}
