// <auto-generated />

namespace Test
{
    partial class TestClass<TOuter>
    {
        protected virtual T TakesRefStructParameter<T, T2>(global::Test.RefStruct refStruct, T2 parameter) where T : TOuter, new() where T2 : struct, global::System.IDisposable
        {
            throw new global::Mocklis.Core.MockMissingException(global::Mocklis.Core.MockType.VirtualMethod, "TestClass", "ITestClass", "TakesRefStructParameter", "TakesRefStructParameter");
        }

        T global::Test.ITestClass<TOuter>.TakesRefStructParameter<T, T2>(global::Test.RefStruct refStruct, T2 parameter) => TakesRefStructParameter<T, T2>(refStruct, parameter);
    }
}