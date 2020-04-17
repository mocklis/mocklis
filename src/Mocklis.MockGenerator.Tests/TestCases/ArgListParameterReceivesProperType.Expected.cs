using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Concat(string s1, string s2, __arglist);
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected virtual string Concat(string s1, string s2, RuntimeArgumentHandle arglist)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "Concat", "Concat");
        }

        string ITestClass.Concat(string s1, string s2, __arglist) => Concat(s1, s2, __arglist);
    }
}
