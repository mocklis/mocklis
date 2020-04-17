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

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

    }
}
