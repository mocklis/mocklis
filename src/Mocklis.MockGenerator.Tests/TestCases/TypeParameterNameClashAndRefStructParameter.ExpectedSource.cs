// <auto-generated />

namespace Test
{
    partial class TestClass<T>
    {
        protected virtual void Test<T0>(global::Test.RefStruct refStruct, T outer, T0 parameter)
        {
            throw new global::Mocklis.Core.MockMissingException(global::Mocklis.Core.MockType.VirtualMethod, "TestClass", "ITestClass", "Test", "Test");
        }

        void global::Test.ITestClass<T>.Test<T0>(global::Test.RefStruct refStruct, T outer, T0 parameter) => Test<T0>(refStruct, outer, parameter);

        protected virtual void TestWithConstraint<T0>(global::Test.RefStruct refStruct, T outer, T0 parameter) where T0 : T
        {
            throw new global::Mocklis.Core.MockMissingException(global::Mocklis.Core.MockType.VirtualMethod, "TestClass", "ITestClass", "TestWithConstraint", "TestWithConstraint");
        }

        void global::Test.ITestClass<T>.TestWithConstraint<T0>(global::Test.RefStruct refStruct, T outer, T0 parameter) => TestWithConstraint<T0>(refStruct, outer, parameter);
    }
}