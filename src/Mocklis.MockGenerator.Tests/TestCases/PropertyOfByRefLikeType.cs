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
        RefStruct RefStructProperty { get; set; }
        RefStruct ReadOnlyProperty { get; }
        RefStruct WriteOnlyProperty { set; }
        ref RefStruct RefReadOnlyProperty { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
