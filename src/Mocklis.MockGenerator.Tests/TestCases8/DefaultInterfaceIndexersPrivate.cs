using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        static int _field;

        private int this[int i] => 0;

        private int this[bool b]
        {
            set => _field = value;
        }

        private int this[string s]
        {
            get => 0;
            set => _field = value;
        }
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
