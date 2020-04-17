using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<TOuter>
    {
        void Test<TInner>(TInner parameter)
            where TInner: TOuter;
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass<T> : ITestClass<T>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _test = new TypedMockProvider();

        public ActionMethodMock<TInner> Test<TInner>() where TInner : T
        {
            var key = new[] { typeof(TInner) };
            return (ActionMethodMock<TInner>)_test.GetOrAdd(key, keyString => new ActionMethodMock<TInner>(this, "TestClass", "ITestClass", "Test" + keyString, "Test" + keyString + "()", Strictness.Lenient));
        }

        void ITestClass<T>.Test<TInner>(TInner parameter) => Test<TInner>().Call(parameter);
    }
}
