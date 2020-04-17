#nullable disable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections.Generic;

    public interface ITestClass
    {
        string NormalProperty { get; }
        string this[string s] { get; }
        event EventHandler NonNullableEvent;
        string Method3(string p1, string p2);
        int ValueTypeProperty { get; }
        int? NullableValueTypeProperty { get; }
    }

#nullable enable

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public abstract class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected TestClass()
        {
            NormalProperty = new PropertyMock<string?>(this, "TestClass", "ITestClass", "NormalProperty", "NormalProperty", Strictness.Lenient);
            Item = new IndexerMock<string?, string?>(this, "TestClass", "ITestClass", "this[]", "Item", Strictness.Lenient);
            NonNullableEvent = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "NonNullableEvent", "NonNullableEvent", Strictness.Lenient);
            Method3 = new FuncMethodMock<(string? p1, string? p2), string?>(this, "TestClass", "ITestClass", "Method3", "Method3", Strictness.Lenient);
            ValueTypeProperty = new PropertyMock<int>(this, "TestClass", "ITestClass", "ValueTypeProperty", "ValueTypeProperty", Strictness.Lenient);
            NullableValueTypeProperty = new PropertyMock<int?>(this, "TestClass", "ITestClass", "NullableValueTypeProperty", "NullableValueTypeProperty", Strictness.Lenient);
        }

        public PropertyMock<string?> NormalProperty { get; }

        string? ITestClass.NormalProperty => NormalProperty.Value;

        public IndexerMock<string?, string?> Item { get; }

        string? ITestClass.this[string? s] => Item[s];

        public EventMock<EventHandler> NonNullableEvent { get; }

        event EventHandler? ITestClass.NonNullableEvent { add => NonNullableEvent.Add(value); remove => NonNullableEvent.Remove(value); }

        public FuncMethodMock<(string? p1, string? p2), string?> Method3 { get; }

        string? ITestClass.Method3(string? p1, string? p2) => Method3.Call((p1, p2));

        public PropertyMock<int> ValueTypeProperty { get; }

        int ITestClass.ValueTypeProperty => ValueTypeProperty.Value;

        public PropertyMock<int?> NullableValueTypeProperty { get; }

        int? ITestClass.NullableValueTypeProperty => NullableValueTypeProperty.Value;
    }
}
