using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        T ReturnsNewType<T>();
        void UsesNewType<T>(T parameter);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        private readonly TypedMockProvider _returnsNewType = new TypedMockProvider();

        public FuncMethodMock<T> ReturnsNewType<T>()
        {
            var key = new[] { typeof(T) };
            return (FuncMethodMock<T>)_returnsNewType.GetOrAdd(key, keyString => new FuncMethodMock<T>(this, "TestClass", "ITestClass", "ReturnsNewType" + keyString, "ReturnsNewType" + keyString + "()"));
        }

        T ITestClass.ReturnsNewType<T>() => ReturnsNewType<T>().Call();

        private readonly TypedMockProvider _usesNewType = new TypedMockProvider();

        public ActionMethodMock<T> UsesNewType<T>()
        {
            var key = new[] { typeof(T) };
            return (ActionMethodMock<T>)_usesNewType.GetOrAdd(key, keyString => new ActionMethodMock<T>(this, "TestClass", "ITestClass", "UsesNewType" + keyString, "UsesNewType" + keyString + "()"));
        }

        void ITestClass.UsesNewType<T>(T parameter) => UsesNewType<T>().Call(parameter);
    }
}
