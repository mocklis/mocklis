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
    }
}
