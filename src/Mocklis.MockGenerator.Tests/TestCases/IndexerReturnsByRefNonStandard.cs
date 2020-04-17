using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int this[int i] { get; }
        ref readonly int this[string s] { get; }
    }

    [MocklisClass(MockReturnsByRef = true, MockReturnsByRefReadonly = false)]
    public class TestClass : ITestClass
    {
    }
}
