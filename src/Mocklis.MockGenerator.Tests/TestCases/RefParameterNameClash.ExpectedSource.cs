// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        public global::Mocklis.Core.FuncMethodMock<(int tmp, int score), int> UpdateScore { get; }

        void global::Test.ITestClass.UpdateScore(int tmp, ref int score)
        {
            score = UpdateScore.Call((tmp, score));
        }

        public global::Mocklis.Core.FuncMethodMock<(string action, int tmp), int> UpdateScore0 { get; }

        void global::Test.ITestClass.UpdateScore(string action, ref int tmp)
        {
            tmp = UpdateScore0.Call((action, tmp));
        }

        public TestClass() : base()
        {
            this.UpdateScore = new global::Mocklis.Core.FuncMethodMock<(int tmp, int score), int>(this, "TestClass", "ITestClass", "UpdateScore", "UpdateScore", global::Mocklis.Core.Strictness.Lenient);
            this.UpdateScore0 = new global::Mocklis.Core.FuncMethodMock<(string action, int tmp), int>(this, "TestClass", "ITestClass", "UpdateScore", "UpdateScore0", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
