// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        public global::Mocklis.Core.PropertyMock<int> GetAndSet { get; }

        int global::Test.ITestClass.GetAndSet { get => GetAndSet.Value; set => GetAndSet.Value = value; }

        public global::Mocklis.Core.PropertyMock<int> SetOnly { get; }

        int global::Test.ITestClass.SetOnly { set => SetOnly.Value = value; }

        public global::Mocklis.Core.PropertyMock<int> GetOnly { get; }

        int global::Test.ITestClass.GetOnly => GetOnly.Value;

        public TestClass() : base()
        {
            this.GetAndSet = new global::Mocklis.Core.PropertyMock<int>(this, "TestClass", "ITestClass", "GetAndSet", "GetAndSet", global::Mocklis.Core.Strictness.Lenient);
            this.SetOnly = new global::Mocklis.Core.PropertyMock<int>(this, "TestClass", "ITestClass", "SetOnly", "SetOnly", global::Mocklis.Core.Strictness.Lenient);
            this.GetOnly = new global::Mocklis.Core.PropertyMock<int>(this, "TestClass", "ITestClass", "GetOnly", "GetOnly", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
