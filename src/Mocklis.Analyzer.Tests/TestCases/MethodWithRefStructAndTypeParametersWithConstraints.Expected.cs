using System;
using Mocklis.Core;

namespace Test
{
    public ref struct RefStruct
    {
        public int Test { get; set;}
    }

    public interface ITestClass<TOuter>
    {
        T TakesRefStructParameter<T, T2>(RefStruct refStruct, T2 parameter)
            where T:TOuter, new()
            where T2:struct, IDisposable;
    }

    [MocklisClass]
    public class TestClass<TOuter> : ITestClass<TOuter>
    {
        protected virtual T TakesRefStructParameter<T, T2>(RefStruct refStruct, T2 parameter)
            where T : TOuter, new()
            where T2 : struct, IDisposable
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "TakesRefStructParameter", "TakesRefStructParameter");
        }

        T ITestClass<TOuter>.TakesRefStructParameter<T, T2>(RefStruct refStruct, T2 parameter) => TakesRefStructParameter<T, T2>(refStruct, parameter);
    }
}
