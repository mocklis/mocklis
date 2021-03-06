using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public ref struct RefStruct
    {
        int Test { get; set;}
    }

    public interface ITestClass<TOuter>
    {
        void Test<T>(RefStruct refStruct, TOuter outer, T parameter);
        void TestWithConstraint<T>(RefStruct refStruct, TOuter outer, T parameter) where T : TOuter;
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass<T> : ITestClass<T>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

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
    }
}
