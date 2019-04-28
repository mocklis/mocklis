using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int Item2, string Item5] { get; set; }
        int this[int Item2, int Item2_] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
