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
        T TakesRefStructParameter<T>(RefStruct parameter);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        protected virtual T TakesRefStructParameter<T>(RefStruct parameter)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "TakesRefStructParameter", "TakesRefStructParameter");
        }

        T ITestClass.TakesRefStructParameter<T>(RefStruct parameter) => TakesRefStructParameter<T>(parameter);
    }
}
