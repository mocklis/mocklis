// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        public global::Mocklis.Core.IndexerMock<int, int> Item { get; }

        int global::Test.ITestClass.this[int Item_] { get => Item[Item_]; set => Item[Item_] = value; }

        public global::Mocklis.Core.IndexerMock<(int Item, int OtherItem), int> Item0 { get; }

        int global::Test.ITestClass.this[int Item, int OtherItem] { get => Item0[(Item, OtherItem)]; set => Item0[(Item, OtherItem)] = value; }

        public TestClass() : base()
        {
            this.Item = new global::Mocklis.Core.IndexerMock<int, int>(this, "TestClass", "ITestClass", "this[]", "Item", global::Mocklis.Core.Strictness.Lenient);
            this.Item0 = new global::Mocklis.Core.IndexerMock<(int Item, int OtherItem), int>(this, "TestClass", "ITestClass", "this[]", "Item0", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
