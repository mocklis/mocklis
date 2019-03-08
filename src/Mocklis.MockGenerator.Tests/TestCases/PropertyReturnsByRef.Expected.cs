using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef { get; }
        ref readonly int ReturnsByRefReadonly { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            ReturnsByRefReadonly = new PropertyMock<int>(this, "TestClass", "ITestClass", "ReturnsByRefReadonly", "ReturnsByRefReadonly");
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
