using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        [IndexerName("Item2_")]
        int this[int Item2, int otherItem] { get; set; }
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Item2_ = new IndexerMock<(int Item2_0, int otherItem), int>(this, "TestClass", "ITestClass", "this[]", "Item2_", Strictness.Lenient);
        }

        public IndexerMock<(int Item2_0, int otherItem), int> Item2_ { get; }

        int ITestClass.this[int Item2_0, int otherItem] { get => Item2_[(Item2_0, otherItem)]; set => Item2_[(Item2_0, otherItem)] = value; }
    }
}
