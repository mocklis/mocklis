using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int Item] { get; set; }
        int this[int Item, int OtherItem] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            Item = new IndexerMock<int, int>(this, "TestClass", "ITestClass", "this[]", "Item");
            Item0 = new IndexerMock<(int Item, int OtherItem), int>(this, "TestClass", "ITestClass", "this[]", "Item0");
        }

        public IndexerMock<int, int> Item { get; }

        int ITestClass.this[int Item_] { get => Item[Item_]; set => Item[Item_] = value; }

        public IndexerMock<(int Item, int OtherItem), int> Item0 { get; }

        int ITestClass.this[int Item, int OtherItem] { get => Item0[(Item, OtherItem)]; set => Item0[(Item, OtherItem)] = value; }
    }
}
