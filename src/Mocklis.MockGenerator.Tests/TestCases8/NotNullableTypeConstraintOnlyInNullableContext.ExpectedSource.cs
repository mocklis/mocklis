// <auto-generated />

namespace Test
{
    partial class TestClass<T>
    {
        public global::Mocklis.Core.FuncMethodMock<U, T> Test { get; }

        T global::Test.ITestClass<T>.Test(U item) => Test.Call(item);

        protected TestClass() : base()
        {
            this.Test = new global::Mocklis.Core.FuncMethodMock<U, T>(this, "TestClass", "ITestClass", "Test", "Test", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
