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
        protected virtual string Concat(string arglist, RuntimeArgumentHandle arglist0)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "Concat", "Concat");
        }

        string ITestClass.Concat(string arglist, __arglist) => Concat(arglist, __arglist);
    }
}
