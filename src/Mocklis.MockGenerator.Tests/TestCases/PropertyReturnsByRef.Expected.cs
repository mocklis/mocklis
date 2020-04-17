using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef { get; }
        ref readonly int ReturnsByRefReadonly { get; }
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            ReturnsByRefReadonly = new PropertyMock<int>(this, "TestClass", "ITestClass", "ReturnsByRefReadonly", "ReturnsByRefReadonly", Strictness.Lenient);
        }

        protected virtual ref int ReturnsByRef()
        {
            throw new MockMissingException(MockType.VirtualPropertyGet, "TestClass", "ITestClass", "ReturnsByRef", "ReturnsByRef");
        }

        ref int ITestClass.ReturnsByRef => ref ReturnsByRef();

        public PropertyMock<int> ReturnsByRefReadonly { get; }

        ref readonly int ITestClass.ReturnsByRefReadonly => ref ByRef<int>.Wrap(ReturnsByRefReadonly.Value);
    }
}
