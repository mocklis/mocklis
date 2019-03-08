using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<TOuter>
    {
        void Test<TInner>(TInner parameter)
            where TInner: TOuter;
    }

    [MocklisClass]
    public class TestClass<T> : ITestClass<T>
    {
        private readonly TypedMockProvider _test = new TypedMockProvider();

        public ActionMethodMock<TInner> Test<TInner>() where TInner : T
        {
            var key = new[] { typeof(TInner) };
            return (ActionMethodMock<TInner>)_test.GetOrAdd(key, keyString => new ActionMethodMock<TInner>(this, "TestClass", "ITestClass", "Test" + keyString, "Test" + keyString + "()"));
        }

        void ITestClass<T>.Test<TInner>(TInner parameter) => Test<TInner>().Call(parameter);
    }
}
