using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        bool TryParse1(string tmp, out int result);
        bool TryParse2(string source, out int tmp);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            TryParse1 = new FuncMethodMock<string, (bool returnValue, int result)>(this, "TestClass", "ITestClass", "TryParse1", "TryParse1", Strictness.Lenient);
            TryParse2 = new FuncMethodMock<string, (bool returnValue, int tmp)>(this, "TestClass", "ITestClass", "TryParse2", "TryParse2", Strictness.Lenient);
        }

        public FuncMethodMock<string, (bool returnValue, int result)> TryParse1 { get; }

        bool ITestClass.TryParse1(string tmp, out int result)
        {
            var tmp0 = TryParse1.Call(tmp);
            result = tmp0.result;
            return tmp0.returnValue;
        }

        public FuncMethodMock<string, (bool returnValue, int tmp)> TryParse2 { get; }

        bool ITestClass.TryParse2(string source, out int tmp)
        {
            var tmp0 = TryParse2.Call(source);
            tmp = tmp0.tmp;
            return tmp0.returnValue;
        }
    }
}
