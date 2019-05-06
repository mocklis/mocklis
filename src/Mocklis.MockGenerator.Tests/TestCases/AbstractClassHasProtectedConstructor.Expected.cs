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
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected TestClass()
        {
            Test = new PropertyMock<int>(this, "TestClass", "ITestClass", "Test", "Test");
        }

        public PropertyMock<int> Test { get; }

        int ITestClass.Test => Test.Value;
    }
}
