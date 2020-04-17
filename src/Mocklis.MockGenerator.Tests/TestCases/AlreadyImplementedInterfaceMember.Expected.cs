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

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : BaseClass, ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Test = new FuncMethodMock<int, string>(this, "TestClass", "ITestClass", "Test", "Test", Strictness.Lenient);
        }

        public FuncMethodMock<int, string> Test { get; }

        string ITestClass.Test(int i) => Test.Call(i);
    }
}
