using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<TOuter>
    {
        void Test<T>(TOuter outer, T parameter);
        void TestWithConstraint<T>(TOuter outer, T parameter) where T : TOuter;
    }

    [MocklisClass]
    public class TestClass<T> : ITestClass<T>
    {
        private readonly TypedMockProvider _test = new TypedMockProvider();

        public ActionMethodMock<(T outer, T0 parameter)> Test<T0>()
        {
            var key = new[] { typeof(T0) };
            return (ActionMethodMock<(T outer, T0 parameter)>)_test.GetOrAdd(key, keyString => new ActionMethodMock<(T outer, T0 parameter)>(this, "TestClass", "ITestClass", "Test" + keyString, "Test" + keyString + "()"));
        }

        void ITestClass<T>.Test<T0>(T outer, T0 parameter) => Test<T0>().Call((outer, parameter));

        private readonly TypedMockProvider _testWithConstraint = new TypedMockProvider();

        public ActionMethodMock<(T outer, T0 parameter)> TestWithConstraint<T0>() where T0 : T
        {
            var key = new[] { typeof(T0) };
            return (ActionMethodMock<(T outer, T0 parameter)>)_testWithConstraint.GetOrAdd(key, keyString => new ActionMethodMock<(T outer, T0 parameter)>(this, "TestClass", "ITestClass", "TestWithConstraint" + keyString, "TestWithConstraint" + keyString + "()"));
        }

        void ITestClass<T>.TestWithConstraint<T0>(T outer, T0 parameter) => TestWithConstraint<T0>().Call((outer, parameter));
    }
}
