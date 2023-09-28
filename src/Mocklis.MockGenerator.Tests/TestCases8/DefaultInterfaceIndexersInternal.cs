using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        static int _field;

        internal int this[int i] => 0;

        internal int this[bool b]
        {
            set => _field = value;
        }

        internal int this[string s]
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
