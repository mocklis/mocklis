using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T, TResult>
    {
        TResult DoCalculation(T sourceData);
    }

    [MocklisClass]
    public class TestClass<T1> : ITestClass<T1, string>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            DoCalculation = new FuncMethodMock<T1, string>(this, "TestClass", "ITestClass", "DoCalculation", "DoCalculation");
        }

        public FuncMethodMock<T1, string> DoCalculation { get; }

        string ITestClass<T1, string>.DoCalculation(T1 sourceData) => DoCalculation.Call(sourceData);
    }
}
