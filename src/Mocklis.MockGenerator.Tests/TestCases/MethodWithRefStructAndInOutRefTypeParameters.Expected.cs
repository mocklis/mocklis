using System;
using Mocklis.Core;

namespace Test
{
    public ref struct RefStruct
    {
        public int Test { get; set;}
    }

    public interface ITestClass
    {
        void RefStructIn<T>(RefStruct refStruct, in T parameter);
        void RefStructOut<T>(RefStruct refStruct, out T paramater);
        void RefStructRef<T>(RefStruct refStruct, ref T parameter);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected virtual void RefStructIn<T>(RefStruct refStruct, in T parameter)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "RefStructIn", "RefStructIn");
        }

        void ITestClass.RefStructIn<T>(RefStruct refStruct, in T parameter) => RefStructIn<T>(refStruct, parameter);

        protected virtual void RefStructOut<T>(RefStruct refStruct, out T paramater)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "RefStructOut", "RefStructOut");
        }

        void ITestClass.RefStructOut<T>(RefStruct refStruct, out T paramater) => RefStructOut<T>(refStruct, out paramater);

        protected virtual void RefStructRef<T>(RefStruct refStruct, ref T parameter)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "RefStructRef", "RefStructRef");
        }

        void ITestClass.RefStructRef<T>(RefStruct refStruct, ref T parameter) => RefStructRef<T>(refStruct, ref parameter);
    }
}
