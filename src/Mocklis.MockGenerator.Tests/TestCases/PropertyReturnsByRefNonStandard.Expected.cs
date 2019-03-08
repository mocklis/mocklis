using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef { get; }
        ref readonly int ReturnsByRefReadonly { get; }
    }

    [MocklisClass(MockReturnsByRef = true, MockReturnsByRefReadonly = false)]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            ReturnsByRef = new PropertyMock<int>(this, "TestClass", "ITestClass", "ReturnsByRef", "ReturnsByRef");
        }

        public PropertyMock<int> ReturnsByRef { get; }

        ref int ITestClass.ReturnsByRef => ref ByRef<int>.Wrap(ReturnsByRef.Value);

        protected virtual ref int ReturnsByRefReadonly()
        {
            throw new MockMissingException(MockType.VirtualPropertyGet, "TestClass", "ITestClass", "ReturnsByRefReadonly", "ReturnsByRefReadonly");
        }

        ref readonly int ITestClass.ReturnsByRefReadonly => ref ReturnsByRefReadonly();
    }
}
