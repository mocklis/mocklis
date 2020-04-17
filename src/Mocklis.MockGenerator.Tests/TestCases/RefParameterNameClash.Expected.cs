using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void UpdateScore(int tmp, ref int score);
        void UpdateScore(string action, ref int tmp);
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            UpdateScore = new FuncMethodMock<(int tmp, int score), int>(this, "TestClass", "ITestClass", "UpdateScore", "UpdateScore", Strictness.Lenient);
            UpdateScore0 = new FuncMethodMock<(string action, int tmp), int>(this, "TestClass", "ITestClass", "UpdateScore", "UpdateScore0", Strictness.Lenient);
        }

        public FuncMethodMock<(int tmp, int score), int> UpdateScore { get; }

        void ITestClass.UpdateScore(int tmp, ref int score)
        {
            score = UpdateScore.Call((tmp, score));
        }

        public FuncMethodMock<(string action, int tmp), int> UpdateScore0 { get; }

        void ITestClass.UpdateScore(string action, ref int tmp)
        {
            tmp = UpdateScore0.Call((action, tmp));
        }
    }
}
