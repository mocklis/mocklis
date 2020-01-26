using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        static int _field;

        public int this[int i] => 0;

        public int this[bool b]
        {
            set => _field = value;
        }

        public int this[string s]
        {
            get => 0;
            set => _field = value;
        }

        public int this[int i, bool b] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Item = new IndexerMock<(int i, bool b), int>(this, "TestClass", "ITestClass", "this[]", "Item", Strictness.Lenient);
        }

        public IndexerMock<(int i, bool b), int> Item { get; }

        int ITestClass.this[int i, bool b] { get => Item[(i, b)]; set => Item[(i, b)] = value; }
    }
}
