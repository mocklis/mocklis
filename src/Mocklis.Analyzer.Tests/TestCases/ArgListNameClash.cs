using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Concat(string arglist, __arglist);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
