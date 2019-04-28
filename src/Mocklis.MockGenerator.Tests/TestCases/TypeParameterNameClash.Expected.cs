using System;
using Mocklis.Core;

namespace Test
{
    public ref struct RefStruct
    {
        public int Test { get; }

        public RefStruct(int test)
        {
            Test = test;
        }
    }

    public interface ITestClass<TOuter>
    {
        void Test<T>(RefStruct refStruct, TOuter outer, T parameter);
        void TestWithConstraint<T>(RefStruct refStruct, TOuter outer, T parameter) where T : TOuter;
        ref readonly T TestWithRef<T>();
    }

    [MocklisClass]
    public class TestClass<T> : ITestClass<T>
    {
        protected virtual void Test<T0>(RefStruct refStruct, T outer, T0 parameter)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "Test", "Test");
        }

        void ITestClass<T>.Test<T0>(RefStruct refStruct, T outer, T0 parameter) => Test<T0>(refStruct, outer, parameter);

        protected virtual void TestWithConstraint<T0>(RefStruct refStruct, T outer, T0 parameter) where T0 : T
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "TestWithConstraint", "TestWithConstraint");
        }

        void ITestClass<T>.TestWithConstraint<T0>(RefStruct refStruct, T outer, T0 parameter) => TestWithConstraint<T0>(refStruct, outer, parameter);

        private readonly TypedMockProvider _testWithRef = new TypedMockProvider();

        public FuncMethodMock<T0> TestWithRef<T0>()
        {
            var key = new[] { typeof(T0) };
            return (FuncMethodMock<T0>)_testWithRef.GetOrAdd(key, keyString => new FuncMethodMock<T0>(this, "TestClass", "ITestClass", "TestWithRef" + keyString, "TestWithRef" + keyString + "()"));
        }

        ref readonly T0 ITestClass<T>.TestWithRef<T0>() => ref ByRef<T0>.Wrap(TestWithRef<T0>().Call());
    }
}
