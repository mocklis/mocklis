using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Concat(string s1, string s2, __arglist);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        protected virtual string Concat(string s1, string s2, RuntimeArgumentHandle arglist)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "Concat", "Concat");
        }

        string ITestClass.Concat(string s1, string s2, __arglist) => Concat(s1, s2, __arglist);
    }
}
