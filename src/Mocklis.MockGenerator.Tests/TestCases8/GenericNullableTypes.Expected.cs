#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass<T> where T : class
    {
        T? Property { get; set; }
        T? this[T? p1, T? p2] { get; set; }
        T? Test(T? item);
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public abstract class TestClass<T> : ITestClass<T> where T : class
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected TestClass()
        {
            Property = new PropertyMock<T?>(this, "TestClass", "ITestClass", "Property", "Property", Strictness.Lenient);
            Item = new IndexerMock<(T? p1, T? p2), T?>(this, "TestClass", "ITestClass", "this[]", "Item", Strictness.Lenient);
            Test = new FuncMethodMock<T?, T?>(this, "TestClass", "ITestClass", "Test", "Test", Strictness.Lenient);
        }

        public PropertyMock<T?> Property { get; }
        T? ITestClass<T>.Property { get => Property.Value; set => Property.Value = value; }
        public IndexerMock<(T? p1, T? p2), T?> Item { get; }

        T? ITestClass<T>.this[T? p1, T? p2] { get => Item[(p1, p2)]; set => Item[(p1, p2)] = value; }

        public FuncMethodMock<T?, T?> Test { get; }

        T? ITestClass<T>.Test(T? item) => Test.Call(item);
    }
}
