// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        private readonly global::Mocklis.Core.TypedMockProvider _test = new global::Mocklis.Core.TypedMockProvider();

        public global::Mocklis.Core.ActionMethodMock<U> Test<U>()
        {
            var key = new[] { typeof(U) };
            return (global::Mocklis.Core.ActionMethodMock<U>)_test.GetOrAdd(key, keyString => new global::Mocklis.Core.ActionMethodMock<U>(this, "TestClass", "ITestClass", "Test" + keyString, "Test" + keyString, global::Mocklis.Core.Strictness.Lenient));
        }

        void global::Test.ITestClass<int>.Test<U>(U parameter) => Test<U>().Call(parameter);
    }
}
