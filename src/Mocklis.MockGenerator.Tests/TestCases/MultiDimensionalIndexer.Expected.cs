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
        public TestClass()
        {
            Item = new IndexerMock<(int i, string s), int>(this, "TestClass", "ITestClass", "this[]", "Item");
            Item0 = new IndexerMock<(string s, string s2), int>(this, "TestClass", "ITestClass", "this[]", "Item0");
            Item1 = new IndexerMock<(char c, int i, string s), int>(this, "TestClass", "ITestClass", "this[]", "Item1");
        }

        public IndexerMock<(int i, string s), int> Item { get; }

        int ITestClass.this[int i, string s] { get => Item[(i, s)]; set => Item[(i, s)] = value; }

        public IndexerMock<(string s, string s2), int> Item0 { get; }

        int ITestClass.this[string s, string s2] { set => Item0[(s, s2)] = value; }

        public IndexerMock<(char c, int i, string s), int> Item1 { get; }

        int ITestClass.this[char c, int i, string s] => Item1[(c, i, s)];
    }
}
