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
    }
}
