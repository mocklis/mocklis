using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T>
    {
        void Test<U>(U parameter) where U : T;
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass<int>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _test = new TypedMockProvider();

        public ActionMethodMock<U> Test<U>()
        {
            var key = new[] { typeof(U) };
            return (ActionMethodMock<U>)_test.GetOrAdd(key, keyString => new ActionMethodMock<U>(this, "TestClass", "ITestClass", "Test" + keyString, "Test" + keyString + "()", Strictness.Lenient));
        }

        void ITestClass<int>.Test<U>(U parameter) => Test<U>().Call(parameter);
    }
}
