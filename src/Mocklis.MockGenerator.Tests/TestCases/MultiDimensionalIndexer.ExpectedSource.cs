// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        public global::Mocklis.Core.IndexerMock<(int i, string s), int> Item { get; }

        int global::Test.ITestClass.this[int i, string s] { get => Item[(i, s)]; set => Item[(i, s)] = value; }

        public global::Mocklis.Core.IndexerMock<(string s, string s2), int> Item0 { get; }

        int global::Test.ITestClass.this[string s, string s2] { set => Item0[(s, s2)] = value; }

        public global::Mocklis.Core.IndexerMock<(char c, int i, string s), int> Item1 { get; }

        int global::Test.ITestClass.this[char c, int i, string s] => Item1[(c, i, s)];

        public TestClass() : base()
        {
            this.Item = new global::Mocklis.Core.IndexerMock<(int i, string s), int>(this, "TestClass", "ITestClass", "this[]", "Item", global::Mocklis.Core.Strictness.Lenient);
            this.Item0 = new global::Mocklis.Core.IndexerMock<(string s, string s2), int>(this, "TestClass", "ITestClass", "this[]", "Item0", global::Mocklis.Core.Strictness.Lenient);
            this.Item1 = new global::Mocklis.Core.IndexerMock<(char c, int i, string s), int>(this, "TestClass", "ITestClass", "this[]", "Item1", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
