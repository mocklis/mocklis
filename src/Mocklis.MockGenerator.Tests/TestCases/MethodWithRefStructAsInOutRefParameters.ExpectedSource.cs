// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        protected virtual void RefStructIn(in global::Test.RefStruct parameter)
        {
            throw new global::Mocklis.Core.MockMissingException(global::Mocklis.Core.MockType.VirtualMethod, "TestClass", "ITestClass", "RefStructIn", "RefStructIn");
        }

        void global::Test.ITestClass.RefStructIn(in global::Test.RefStruct parameter) => RefStructIn(parameter);

        protected virtual void RefStructOut(out global::Test.RefStruct parameter)
        {
            throw new global::Mocklis.Core.MockMissingException(global::Mocklis.Core.MockType.VirtualMethod, "TestClass", "ITestClass", "RefStructOut", "RefStructOut");
        }

        void global::Test.ITestClass.RefStructOut(out global::Test.RefStruct parameter) => RefStructOut(out parameter);

        protected virtual void RefStructRef(ref global::Test.RefStruct parameter)
        {
            throw new global::Mocklis.Core.MockMissingException(global::Mocklis.Core.MockType.VirtualMethod, "TestClass", "ITestClass", "RefStructRef", "RefStructRef");
        }

        void global::Test.ITestClass.RefStructRef(ref global::Test.RefStruct parameter) => RefStructRef(ref parameter);
    }
}
