using System;
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
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

    }
}
