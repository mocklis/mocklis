using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int Item] { get; set; }
        int this[int Item, int OtherItem] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
