using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public ref struct RefStruct
    {
        public int Test { get; set;}
    }

    public interface ITestClass
    {
        RefStruct this[int i] { get; set; }
        RefStruct this[string s] { get; }
        RefStruct this[int i, string s] { set; }
        ref RefStruct this[bool b] { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
