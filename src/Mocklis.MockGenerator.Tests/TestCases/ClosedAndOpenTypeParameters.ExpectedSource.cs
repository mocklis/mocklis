// <auto-generated />

namespace Test
{
    partial class TestClass<T1>
    {
        public global::Mocklis.Core.FuncMethodMock<T1, string> DoCalculation { get; }

        string global::Test.ITestClass<T1, string>.DoCalculation(T1 sourceData) => DoCalculation.Call(sourceData);

        public TestClass() : base()
        {
            this.DoCalculation = new global::Mocklis.Core.FuncMethodMock<T1, string>(this, "TestClass", "ITestClass", "DoCalculation", "DoCalculation", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
