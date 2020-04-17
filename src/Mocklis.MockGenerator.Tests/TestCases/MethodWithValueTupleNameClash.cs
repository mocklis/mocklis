using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void Test(int Item2, int AnotherItem);
        int Test2(out int Item1);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
