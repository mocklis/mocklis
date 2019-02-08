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
        RefStruct ReturnsRefStruct();
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        protected virtual RefStruct ReturnsRefStruct()
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "ReturnsRefStruct", "ReturnsRefStruct");
        }

        RefStruct ITestClass.ReturnsRefStruct() => ReturnsRefStruct();
    }
}
