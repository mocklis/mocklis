using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int Test { get; }
    }

    public class BaseClass
    {
        public BaseClass(int i)
        {
        }

        protected BaseClass(string s)
        {
        }

        private BaseClass(char c)
        {
        }
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : BaseClass, ITestClass
    {
    }
}
