using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        bool TryParse(string source, out int result);
        int Test(in int t1, in int t2);
        void Mutate(ref string value);
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            TryParse = new FuncMethodMock<string, (bool returnValue, int result)>(this, "TestClass", "ITestClass", "TryParse", "TryParse", Strictness.Lenient);
            Test = new FuncMethodMock<(int t1, int t2), int>(this, "TestClass", "ITestClass", "Test", "Test", Strictness.Lenient);
            Mutate = new FuncMethodMock<string, string>(this, "TestClass", "ITestClass", "Mutate", "Mutate", Strictness.Lenient);
        }

        public FuncMethodMock<string, (bool returnValue, int result)> TryParse { get; }

        bool ITestClass.TryParse(string source, out int result)
        {
            var tmp = TryParse.Call(source);
            result = tmp.result;
            return tmp.returnValue;
        }

        public FuncMethodMock<(int t1, int t2), int> Test { get; }

        int ITestClass.Test(in int t1, in int t2) => Test.Call((t1, t2));

        public FuncMethodMock<string, string> Mutate { get; }

        void ITestClass.Mutate(ref string value)
        {
            value = Mutate.Call(value);
        }
    }
}
