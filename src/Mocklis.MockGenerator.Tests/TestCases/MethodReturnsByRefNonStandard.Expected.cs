using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef();
        ref readonly int ReturnsByRefReadonly();
    }

    [MocklisClass(MockReturnsByRef = true, MockReturnsByRefReadonly = false)]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            ReturnsByRef = new FuncMethodMock<int>(this, "TestClass", "ITestClass", "ReturnsByRef", "ReturnsByRef");
        }

        public FuncMethodMock<int> ReturnsByRef { get; }

        ref int ITestClass.ReturnsByRef() => ref ByRef<int>.Wrap(ReturnsByRef.Call());

        protected virtual ref int ReturnsByRefReadonly()
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "ReturnsByRefReadonly", "ReturnsByRefReadonly");
        }

        ref readonly int ITestClass.ReturnsByRefReadonly() => ref ReturnsByRefReadonly();
    }
}
