using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef { get; }
        ref readonly int ReturnsByRefReadonly { get; }
    }

    [MocklisClass(MockReturnsByRef = true, MockReturnsByRefReadonly = false)]
    public class TestClass : ITestClass
    {
    }
}
