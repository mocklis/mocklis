using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        T2 Test<T1, T2, T3, T4>(T1 parameter, T3 parameter2, T4 anotherParameter)
            where T1 : unmanaged, ICloneable
            where T2 : class, IDisposable, new()
            where T3 : struct
            where T4 : new();
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _test = new TypedMockProvider();

        public FuncMethodMock<(T1 parameter, T3 parameter2, T4 anotherParameter), T2> Test<T1, T2, T3, T4>()
            where T1 : unmanaged, ICloneable
            where T2 : class, IDisposable, new()
            where T3 : struct
            where T4 : new()
        {
            var key = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
            return (FuncMethodMock<(T1 parameter, T3 parameter2, T4 anotherParameter), T2>)_test.GetOrAdd(key, keyString => new FuncMethodMock<(T1 parameter, T3 parameter2, T4 anotherParameter), T2>(this, "TestClass", "ITestClass", "Test" + keyString, "Test" + keyString + "()", Strictness.Lenient));
        }

        T2 ITestClass.Test<T1, T2, T3, T4>(T1 parameter, T3 parameter2, T4 anotherParameter) => Test<T1, T2, T3, T4>().Call((parameter, parameter2, anotherParameter));
    }
}
