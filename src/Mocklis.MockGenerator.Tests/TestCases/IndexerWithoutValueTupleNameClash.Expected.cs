using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int Item0, int Item2, int Item02, int Item, int ItemTest] { get; set; }
        int this[int Item0] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            Item = new IndexerMock<(int Item0, int Item2, int Item02, int Item_, int ItemTest), int>(this, "TestClass", "ITestClass", "this[]", "Item");
            Item0 = new IndexerMock<int, int>(this, "TestClass", "ITestClass", "this[]", "Item0");
        }

        public IndexerMock<(int Item0, int Item2, int Item02, int Item_, int ItemTest), int> Item { get; }

        int ITestClass.this[int Item0, int Item2, int Item02, int Item_, int ItemTest] { get => Item[(Item0, Item2, Item02, Item_, ItemTest)]; set => Item[(Item0, Item2, Item02, Item_, ItemTest)] = value; }

        public IndexerMock<int, int> Item0 { get; }

        int ITestClass.this[int Item0_] { get => Item0[Item0_]; set => Item0[Item0_] = value; }
    }
}
