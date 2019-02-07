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
        RefStruct RefStructProperty { get; set; }
        RefStruct ReadOnlyProperty { get; }
        RefStruct WriteOnlyProperty { set; }
        ref RefStruct RefReadOnlyProperty { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        protected virtual RefStruct RefStructProperty()
        {
            throw new MockMissingException(MockType.VirtualPropertyGet, "TestClass", "ITestClass", "RefStructProperty", "RefStructProperty");
        }

        protected virtual RefStruct RefStructProperty(RefStruct Value)
        {
            throw new MockMissingException(MockType.VirtualPropertySet, "TestClass", "ITestClass", "RefStructProperty", "RefStructProperty");
        }

        RefStruct ITestClass.RefStructProperty { get => RefStructProperty(); set => RefStructProperty(value); }

        protected virtual RefStruct ReadOnlyProperty()
        {
            throw new MockMissingException(MockType.VirtualPropertyGet, "TestClass", "ITestClass", "ReadOnlyProperty", "ReadOnlyProperty");
        }

        RefStruct ITestClass.ReadOnlyProperty => ReadOnlyProperty();

        protected virtual RefStruct WriteOnlyProperty(RefStruct Value)
        {
            throw new MockMissingException(MockType.VirtualPropertySet, "TestClass", "ITestClass", "WriteOnlyProperty", "WriteOnlyProperty");
        }

        RefStruct ITestClass.WriteOnlyProperty { set => WriteOnlyProperty(value); }

        protected virtual ref RefStruct RefReadOnlyProperty()
        {
            throw new MockMissingException(MockType.VirtualPropertyGet, "TestClass", "ITestClass", "RefReadOnlyProperty", "RefReadOnlyProperty");
        }

        ref RefStruct ITestClass.RefReadOnlyProperty => ref RefReadOnlyProperty();
    }
}
