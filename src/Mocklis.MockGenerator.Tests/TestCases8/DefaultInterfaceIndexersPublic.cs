using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        static int _field;

        public int this[int i] => 0;

        public int this[bool b]
        {
            set => _field = value;
        }

        public int this[string s]
        {
            get => 0;
            set => _field = value;
        }

        public int this[int i, bool b] { get; set; }
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
