using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef();
        ref readonly int ReturnsByRefReadonly();
    }

    [MocklisClass(MockReturnsByRef = true, MockReturnsByRefReadonly = false)]
    public class TestClass : ITestClass
    {
    }
}
