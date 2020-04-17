using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        [IndexerName("TheIndexerName")]
        int this[int i] { get; set; }
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            TheIndexerName = new IndexerMock<int, int>(this, "TestClass", "ITestClass", "this[]", "TheIndexerName", Strictness.Lenient);
        }

        public IndexerMock<int, int> TheIndexerName { get; }

        int ITestClass.this[int i] { get => TheIndexerName[i]; set => TheIndexerName[i] = value; }
    }
}
