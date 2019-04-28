using System;
using System.Runtime.CompilerServices;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        [IndexerName("Item2_")]
        int this[int Item2, int otherItem] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            Item2_ = new IndexerMock<(int Item2_0, int otherItem), int>(this, "TestClass", "ITestClass", "this[]", "Item2_");
        }

        public IndexerMock<(int Item2_0, int otherItem), int> Item2_ { get; }

        int ITestClass.this[int Item2_0, int otherItem] { get => Item2_[(Item2_0, otherItem)]; set => Item2_[(Item2_0, otherItem)] = value; }
    }
}
