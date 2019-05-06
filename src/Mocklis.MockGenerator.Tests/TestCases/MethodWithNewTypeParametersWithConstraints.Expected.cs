using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        T2 Test<T1, T2>(T1 parameter)
            where T1: struct
            where T2 : class, IDisposable, new();
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _test = new TypedMockProvider();

        public FuncMethodMock<T1, T2> Test<T1, T2>()
            where T1 : struct
            where T2 : class, IDisposable, new()
        {
            var key = new[] { typeof(T1), typeof(T2) };
            return (FuncMethodMock<T1, T2>)_test.GetOrAdd(key, keyString => new FuncMethodMock<T1, T2>(this, "TestClass", "ITestClass", "Test" + keyString, "Test" + keyString + "()"));
        }

        T2 ITestClass.Test<T1, T2>(T1 parameter) => Test<T1, T2>().Call(parameter);
    }
}
