using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int this[int i] { get; }
        ref readonly int this[string s] { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Item0 = new IndexerMock<string, int>(this, "TestClass", "ITestClass", "this[]", "Item0");
        }

        protected virtual ref int Item(int i)
        {
            throw new MockMissingException(MockType.VirtualIndexerGet, "TestClass", "ITestClass", "this[]", "Item");
        }

        ref int ITestClass.this[int i] => ref Item(i);

        public IndexerMock<string, int> Item0 { get; }

        ref readonly int ITestClass.this[string s] => ref ByRef<int>.Wrap(Item0[s]);
    }
}
