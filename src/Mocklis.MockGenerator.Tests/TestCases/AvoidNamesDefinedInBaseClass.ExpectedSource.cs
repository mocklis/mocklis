// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        public global::Mocklis.Core.FuncMethodMock<int, string> Test0 { get; }

        string global::Test.ITestClass.Test(int i) => Test0.Call(i);

        public global::Mocklis.Core.PropertyMock<bool> AnotherTest0 { get; }

        bool global::Test.ITestClass.AnotherTest { get => AnotherTest0.Value; set => AnotherTest0.Value = value; }

        public TestClass() : base()
        {
            this.Test0 = new global::Mocklis.Core.FuncMethodMock<int, string>(this, "TestClass", "ITestClass", "Test", "Test0", global::Mocklis.Core.Strictness.Lenient);
            this.AnotherTest0 = new global::Mocklis.Core.PropertyMock<bool>(this, "TestClass", "ITestClass", "AnotherTest", "AnotherTest0", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
