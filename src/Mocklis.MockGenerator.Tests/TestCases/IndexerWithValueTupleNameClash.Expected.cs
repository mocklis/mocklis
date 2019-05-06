using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int Item2, string Item5] { get; set; }
        int this[int Item2, int Item2_] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Item = new IndexerMock<(int Item2_, string Item5_), int>(this, "TestClass", "ITestClass", "this[]", "Item");
            Item0 = new IndexerMock<(int Item2_0, int Item2_), int>(this, "TestClass", "ITestClass", "this[]", "Item0");
        }

        public IndexerMock<(int Item2_, string Item5_), int> Item { get; }

        int ITestClass.this[int Item2_, string Item5_] { get => Item[(Item2_, Item5_)]; set => Item[(Item2_, Item5_)] = value; }

        public IndexerMock<(int Item2_0, int Item2_), int> Item0 { get; }

        int ITestClass.this[int Item2_0, int Item2_] { get => Item0[(Item2_0, Item2_)]; set => Item0[(Item2_0, Item2_)] = value; }
    }
}
