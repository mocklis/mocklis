using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Combine<T1, T2>(T1 param1, T2 param2);
        string Combine<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
        string Combine<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        private readonly TypedMockProvider _combine = new TypedMockProvider();

        public FuncMethodMock<(T1 param1, T2 param2), string> Combine<T1, T2>()
        {
            var key = new[] { typeof(T1), typeof(T2) };
            return (FuncMethodMock<(T1 param1, T2 param2), string>)_combine.GetOrAdd(key, keyString => new FuncMethodMock<(T1 param1, T2 param2), string>(this, "TestClass", "ITestClass", "Combine" + keyString, "Combine" + keyString + "()"));
        }

        string ITestClass.Combine<T1, T2>(T1 param1, T2 param2) => Combine<T1, T2>().Call((param1, param2));

        private readonly TypedMockProvider _combine0 = new TypedMockProvider();

        public FuncMethodMock<(T1 param1, T2 param2, T3 param3), string> Combine0<T1, T2, T3>()
        {
            var key = new[] { typeof(T1), typeof(T2), typeof(T3) };
            return (FuncMethodMock<(T1 param1, T2 param2, T3 param3), string>)_combine0.GetOrAdd(key, keyString => new FuncMethodMock<(T1 param1, T2 param2, T3 param3), string>(this, "TestClass", "ITestClass", "Combine" + keyString, "Combine0" + keyString + "()"));
        }

        string ITestClass.Combine<T1, T2, T3>(T1 param1, T2 param2, T3 param3) => Combine0<T1, T2, T3>().Call((param1, param2, param3));

        private readonly TypedMockProvider _combine1 = new TypedMockProvider();

        public FuncMethodMock<(T1 param1, T2 param2, T3 param3, T4 param4), string> Combine1<T1, T2, T3, T4>()
        {
            var key = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
            return (FuncMethodMock<(T1 param1, T2 param2, T3 param3, T4 param4), string>)_combine1.GetOrAdd(key, keyString => new FuncMethodMock<(T1 param1, T2 param2, T3 param3, T4 param4), string>(this, "TestClass", "ITestClass", "Combine" + keyString, "Combine1" + keyString + "()"));
        }

        string ITestClass.Combine<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4) => Combine1<T1, T2, T3, T4>().Call((param1, param2, param3, param4));
    }
}