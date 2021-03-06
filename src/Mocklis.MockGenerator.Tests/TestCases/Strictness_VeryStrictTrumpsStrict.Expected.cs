using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int Property { get; set; }
        string this[int i] { get; set; }
        string MyMethod(string param);
        event EventHandler MyEvent;
    }

    [MocklisClass(Strict = false, VeryStrict = true), GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Property = new PropertyMock<int>(this, "TestClass", "ITestClass", "Property", "Property", Strictness.VeryStrict);
            Item = new IndexerMock<int, string>(this, "TestClass", "ITestClass", "this[]", "Item", Strictness.VeryStrict);
            MyMethod = new FuncMethodMock<string, string>(this, "TestClass", "ITestClass", "MyMethod", "MyMethod", Strictness.VeryStrict);
            MyEvent = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "MyEvent", "MyEvent", Strictness.VeryStrict);
        }

        public PropertyMock<int> Property { get; }
        int ITestClass.Property { get => Property.Value; set => Property.Value = value; }
        public IndexerMock<int, string> Item { get; }

        string ITestClass.this[int i] { get => Item[i]; set => Item[i] = value; }

        public FuncMethodMock<string, string> MyMethod { get; }

        string ITestClass.MyMethod(string param) => MyMethod.Call(param);

        public EventMock<EventHandler> MyEvent { get; }

        event EventHandler ITestClass.MyEvent { add => MyEvent.Add(value); remove => MyEvent.Remove(value); }
    }
}
