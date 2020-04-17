using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int this[int Item0, int Item2, int Item02, int Item, int ItemTest] { get; set; }
        int this[int Item0] { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
