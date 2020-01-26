using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        static int _field;

        private protected int this[int i] => 0;

        private protected int this[bool b]
        {
            set => _field = value;
        }

        private protected int this[string s]
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
