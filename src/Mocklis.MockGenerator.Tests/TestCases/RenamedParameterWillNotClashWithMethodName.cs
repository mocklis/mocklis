using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void Item2_(int Item2, int anotherItem);
        int Item3_(ref string Item3);
        int Item4_<T>(ref T Item4);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
