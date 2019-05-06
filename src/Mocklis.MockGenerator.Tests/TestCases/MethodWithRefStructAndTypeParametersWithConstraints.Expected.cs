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
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected virtual T TakesRefStructParameter<T, T2>(RefStruct refStruct, T2 parameter)
            where T : TOuter, new()
            where T2 : struct, IDisposable
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "TakesRefStructParameter", "TakesRefStructParameter");
        }

        T ITestClass<TOuter>.TakesRefStructParameter<T, T2>(RefStruct refStruct, T2 parameter) => TakesRefStructParameter<T, T2>(refStruct, parameter);
    }
}
