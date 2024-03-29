using System;
using System.CodeDom.Compiler;
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
    public [PARTIAL] class TestClass<TOuter> : ITestClass<TOuter>
    {
    }
}
