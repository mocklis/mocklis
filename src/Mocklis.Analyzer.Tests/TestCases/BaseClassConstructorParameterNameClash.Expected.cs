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
        public TestClass(int Test) : base(Test)
        {
            this.Test = new PropertyMock<int>(this, "TestClass", "ITestClass", "Test", "Test");
            Test2 = new PropertyMock<int>(this, "TestClass", "ITestClass", "Test2", "Test2");
        }

        public PropertyMock<int> Test { get; }

        int ITestClass.Test => Test.Value;

        public PropertyMock<int> Test2 { get; }

        int ITestClass.Test2 => Test2.Value;
    }
}