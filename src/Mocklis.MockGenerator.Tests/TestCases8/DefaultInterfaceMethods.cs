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

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
