using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void UpdateScore(int tmp, ref int score);
        void UpdateScore(string action, ref int tmp);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            UpdateScore = new FuncMethodMock<(int tmp, int score), int>(this, "TestClass", "ITestClass", "UpdateScore", "UpdateScore");
            UpdateScore0 = new FuncMethodMock<(string action, int tmp), int>(this, "TestClass", "ITestClass", "UpdateScore", "UpdateScore0");
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
