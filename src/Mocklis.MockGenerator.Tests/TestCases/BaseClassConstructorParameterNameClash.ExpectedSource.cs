// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        public global::Mocklis.Core.PropertyMock<int> Test { get; }

        int global::Test.ITestClass.Test => Test.Value;

        public global::Mocklis.Core.PropertyMock<int> Test2 { get; }

        int global::Test.ITestClass.Test2 => Test2.Value;

        public TestClass(int Test) : base(Test)
        {
            this.Test = new global::Mocklis.Core.PropertyMock<int>(this, "TestClass", "ITestClass", "Test", "Test", global::Mocklis.Core.Strictness.Lenient);
            this.Test2 = new global::Mocklis.Core.PropertyMock<int>(this, "TestClass", "ITestClass", "Test2", "Test2", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
