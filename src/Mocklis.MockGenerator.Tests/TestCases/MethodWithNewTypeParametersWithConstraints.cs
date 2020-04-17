using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        T2 Test<T1, T2, T3, T4>(T1 parameter, T3 parameter2, T4 anotherParameter)
            where T1 : unmanaged, ICloneable
            where T2 : class, IDisposable, new()
            where T3 : struct
            where T4 : new();
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
