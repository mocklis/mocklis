using System;
using System.Runtime.CompilerServices;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        [IndexerName("TheIndexerName")]
        int this[int i] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            TheIndexerName = new IndexerMock<int, int>(this, "TestClass", "ITestClass", "this[]", "TheIndexerName");
        }

        public IndexerMock<int, int> TheIndexerName { get; }

        int ITestClass.this[int i] { get => TheIndexerName[i]; set => TheIndexerName[i] = value; }
    }
}
