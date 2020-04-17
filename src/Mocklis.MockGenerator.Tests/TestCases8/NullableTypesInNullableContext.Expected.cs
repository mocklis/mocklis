#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass
    {
        string? NullableProperty { get; }
        string NormalProperty { get; }
        string? this[int i] { get; }
        string this[bool b] { get; }
        string? this[string? s] { get; }
        string this[int i, string? s] { get; }
        event EventHandler NonNullableEvent;
        event EventHandler? NullableEvent;
        string Method1(string? parameter);
        string? Method2(string parameter);
        string Method3(string p1, string? p2);
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public abstract class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected TestClass()
        {
            NullableProperty = new PropertyMock<string?>(this, "TestClass", "ITestClass", "NullableProperty", "NullableProperty", Strictness.Lenient);
            NormalProperty = new PropertyMock<string>(this, "TestClass", "ITestClass", "NormalProperty", "NormalProperty", Strictness.Lenient);
            Item = new IndexerMock<int, string?>(this, "TestClass", "ITestClass", "this[]", "Item", Strictness.Lenient);
            Item0 = new IndexerMock<bool, string>(this, "TestClass", "ITestClass", "this[]", "Item0", Strictness.Lenient);
            Item1 = new IndexerMock<string?, string?>(this, "TestClass", "ITestClass", "this[]", "Item1", Strictness.Lenient);
            Item2 = new IndexerMock<(int i, string? s), string>(this, "TestClass", "ITestClass", "this[]", "Item2", Strictness.Lenient);
            NonNullableEvent = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "NonNullableEvent", "NonNullableEvent", Strictness.Lenient);
            NullableEvent = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "NullableEvent", "NullableEvent", Strictness.Lenient);
            Method1 = new FuncMethodMock<string?, string>(this, "TestClass", "ITestClass", "Method1", "Method1", Strictness.Lenient);
            Method2 = new FuncMethodMock<string, string?>(this, "TestClass", "ITestClass", "Method2", "Method2", Strictness.Lenient);
            Method3 = new FuncMethodMock<(string p1, string? p2), string>(this, "TestClass", "ITestClass", "Method3", "Method3", Strictness.Lenient);
        }

        public PropertyMock<string?> NullableProperty { get; }

        string? ITestClass.NullableProperty => NullableProperty.Value;

        public PropertyMock<string> NormalProperty { get; }

        string ITestClass.NormalProperty => NormalProperty.Value;

        public IndexerMock<int, string?> Item { get; }

        string? ITestClass.this[int i] => Item[i];

        public IndexerMock<bool, string> Item0 { get; }

        string ITestClass.this[bool b] => Item0[b];

        public IndexerMock<string?, string?> Item1 { get; }

        string? ITestClass.this[string? s] => Item1[s];

        public IndexerMock<(int i, string? s), string> Item2 { get; }

        string ITestClass.this[int i, string? s] => Item2[(i, s)];

        public EventMock<EventHandler> NonNullableEvent { get; }

        event EventHandler ITestClass.NonNullableEvent { add => NonNullableEvent.Add(value); remove => NonNullableEvent.Remove(value); }

        public EventMock<EventHandler> NullableEvent { get; }

        event EventHandler? ITestClass.NullableEvent { add => NullableEvent.Add(value); remove => NullableEvent.Remove(value); }

        public FuncMethodMock<string?, string> Method1 { get; }

        string ITestClass.Method1(string? parameter) => Method1.Call(parameter);

        public FuncMethodMock<string, string?> Method2 { get; }

        string? ITestClass.Method2(string parameter) => Method2.Call(parameter);

        public FuncMethodMock<(string p1, string? p2), string> Method3 { get; }

        string ITestClass.Method3(string p1, string? p2) => Method3.Call((p1, p2));
    }
}
