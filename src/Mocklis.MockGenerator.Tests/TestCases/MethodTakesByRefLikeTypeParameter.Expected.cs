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
        void TakesRefStructParameter(RefStruct parameter);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        protected virtual void TakesRefStructParameter(RefStruct parameter)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "TakesRefStructParameter", "TakesRefStructParameter");
        }

        void ITestClass.TakesRefStructParameter(RefStruct parameter) => TakesRefStructParameter(parameter);
    }
}
