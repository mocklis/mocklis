using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int i, string s] { get; set; }
        int this[string s, string s2] { set; }
        int this[char c, int i, string s] { get; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Item = new IndexerMock<(int i, string s), int>(this, "TestClass", "ITestClass", "this[]", "Item", Strictness.Lenient);
            Item0 = new IndexerMock<(string s, string s2), int>(this, "TestClass", "ITestClass", "this[]", "Item0", Strictness.Lenient);
            Item1 = new IndexerMock<(char c, int i, string s), int>(this, "TestClass", "ITestClass", "this[]", "Item1", Strictness.Lenient);
        }

        public IndexerMock<(int i, string s), int> Item { get; }

        int ITestClass.this[int i, string s] { get => Item[(i, s)]; set => Item[(i, s)] = value; }

        public IndexerMock<(string s, string s2), int> Item0 { get; }

        int ITestClass.this[string s, string s2] { set => Item0[(s, s2)] = value; }

        public IndexerMock<(char c, int i, string s), int> Item1 { get; }

        int ITestClass.this[char c, int i, string s] => Item1[(c, i, s)];
    }
}
