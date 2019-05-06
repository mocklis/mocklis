using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int Test { get; }
    }

    public class BaseClass
    {
        public BaseClass(int i)
        {
        }

        protected BaseClass(string s)
        {
        }

        private BaseClass(char c)
        {
        }
    }

    [MocklisClass]
    public class TestClass : BaseClass, ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass(int i) : base(i)
        {
            Test = new PropertyMock<int>(this, "TestClass", "ITestClass", "Test", "Test");
        }

        public TestClass(string s) : base(s)
        {
            Test = new PropertyMock<int>(this, "TestClass", "ITestClass", "Test", "Test");
        }

        public PropertyMock<int> Test { get; }

        int ITestClass.Test => Test.Value;
    }
}
