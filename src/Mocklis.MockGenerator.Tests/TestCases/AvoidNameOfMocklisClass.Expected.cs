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
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            TestClass0 = new FuncMethodMock<int, string>(this, "TestClass", "ITestClass", "TestClass", "TestClass0");
        }

        public FuncMethodMock<int, string> TestClass0 { get; }

        string ITestClass.TestClass(int i) => TestClass0.Call(i);
    }
}
