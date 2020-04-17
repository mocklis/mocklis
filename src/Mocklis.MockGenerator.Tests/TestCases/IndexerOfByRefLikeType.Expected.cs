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

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected virtual RefStruct Item(int i)
        {
            throw new MockMissingException(MockType.VirtualIndexerGet, "TestClass", "ITestClass", "this[]", "Item");
        }

        protected virtual void Item(int i, RefStruct value)
        {
            throw new MockMissingException(MockType.VirtualIndexerSet, "TestClass", "ITestClass", "this[]", "Item");
        }

        RefStruct ITestClass.this[int i] { get => Item(i); set => Item(i, value); }

        protected virtual RefStruct Item0(string s)
        {
            throw new MockMissingException(MockType.VirtualIndexerGet, "TestClass", "ITestClass", "this[]", "Item0");
        }

        RefStruct ITestClass.this[string s] => Item0(s);

        protected virtual void Item1(int i, string s, RefStruct value)
        {
            throw new MockMissingException(MockType.VirtualIndexerSet, "TestClass", "ITestClass", "this[]", "Item1");
        }

        RefStruct ITestClass.this[int i, string s] { set => Item1(i, s, value); }

        protected virtual ref RefStruct Item2(bool b)
        {
            throw new MockMissingException(MockType.VirtualIndexerGet, "TestClass", "ITestClass", "this[]", "Item2");
        }

        ref RefStruct ITestClass.this[bool b] => ref Item2(b);
    }
}
