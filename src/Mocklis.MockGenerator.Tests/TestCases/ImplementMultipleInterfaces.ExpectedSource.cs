// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        public global::Mocklis.Core.PropertyMock<int> Property1 { get; }

        int global::Test.ITest1.Property1 { get => Property1.Value; set => Property1.Value = value; }

        public global::Mocklis.Core.ActionMethodMock<string> DoSomething { get; }

        void global::Test.ITest2.DoSomething(string task) => DoSomething.Call(task);

        public TestClass() : base()
        {
            this.Property1 = new global::Mocklis.Core.PropertyMock<int>(this, "TestClass", "ITest1", "Property1", "Property1", global::Mocklis.Core.Strictness.Lenient);
            this.DoSomething = new global::Mocklis.Core.ActionMethodMock<string>(this, "TestClass", "ITest2", "DoSomething", "DoSomething", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}