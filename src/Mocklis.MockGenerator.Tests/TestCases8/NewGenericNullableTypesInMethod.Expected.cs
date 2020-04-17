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
        void DoStuff<T>(T? p) where T : class;
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public abstract class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _doStuff = new TypedMockProvider();

        public ActionMethodMock<T?> DoStuff<T>() where T : class
        {
            var key = new[] { typeof(T) };
            return (ActionMethodMock<T?>)_doStuff.GetOrAdd(key, keyString => new ActionMethodMock<T?>(this, "TestClass", "ITestClass", "DoStuff" + keyString, "DoStuff" + keyString + "()", Strictness.Lenient));
        }

        void ITestClass.DoStuff<T>(T? p) where T : class => DoStuff<T>().Call(p);
    }
}
