using System;
using System.Runtime.CompilerServices;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        [IndexerName("TheIndexerName")]
        int this[int TheIndexerName] { get; set; }

        [IndexerName("TheIndexerName")]
        int this[int TheIndexerName, int OtherItem] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            TheIndexerName = new IndexerMock<int, int>(this, "TestClass", "ITestClass", "this[]", "TheIndexerName", Strictness.Lenient);
            TheIndexerName0 = new IndexerMock<(int TheIndexerName, int OtherItem), int>(this, "TestClass", "ITestClass", "this[]", "TheIndexerName0", Strictness.Lenient);
        }

        public IndexerMock<int, int> TheIndexerName { get; }

        int ITestClass.this[int TheIndexerName_] { get => TheIndexerName[TheIndexerName_]; set => TheIndexerName[TheIndexerName_] = value; }

        public IndexerMock<(int TheIndexerName, int OtherItem), int> TheIndexerName0 { get; }

        int ITestClass.this[int TheIndexerName, int OtherItem] { get => TheIndexerName0[(TheIndexerName, OtherItem)]; set => TheIndexerName0[(TheIndexerName, OtherItem)] = value; }
    }
}
