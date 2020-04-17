using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        public void Pub(string e)
        {
        }

        static void Stat(string e)
        {
        }

        internal void Int(string e)
        {
        }

        private void Priv(string e)
        {
        }

        protected void Prot(string e)
        {
        }

        protected internal void ProtInt(string e)
        {
        }

        private protected void PrivProt(string e)
        {
        }

        public void Normal(string e);
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Normal = new ActionMethodMock<string>(this, "TestClass", "ITestClass", "Normal", "Normal", Strictness.Lenient);
        }

        public ActionMethodMock<string> Normal { get; }

        void ITestClass.Normal(string e) => Normal.Call(e);
    }
}
