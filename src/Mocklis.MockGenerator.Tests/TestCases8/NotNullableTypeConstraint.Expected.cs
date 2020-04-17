#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass<T> where T : notnull
    {
        T Test<U>(U item) where U : notnull;
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public abstract class TestClass<T> : ITestClass<T> where T : notnull
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _test = new TypedMockProvider();

        public FuncMethodMock<U, T> Test<U>() where U : notnull
        {
            var key = new[] { typeof(U) };
            return (FuncMethodMock<U, T>)_test.GetOrAdd(key, keyString => new FuncMethodMock<U, T>(this, "TestClass", "ITestClass", "Test" + keyString, "Test" + keyString + "()", Strictness.Lenient));
        }

        T ITestClass<T>.Test<U>(U item) => Test<U>().Call(item);
    }
}
