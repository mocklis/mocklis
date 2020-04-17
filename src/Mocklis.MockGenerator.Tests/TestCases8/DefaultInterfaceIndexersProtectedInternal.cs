using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        static int _field;

        protected internal int this[int i] => 0;

        protected internal int this[bool b]
        {
            set => _field = value;
        }

        protected internal int this[string s]
        {
            get => 0;
            set => _field = value;
        }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
