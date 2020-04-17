using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        public event EventHandler Pub
        {
            add
            {
                throw new ArgumentException();
            }
            remove
            {
                throw new ArgumentException();
            }
        }

        static event EventHandler Stat
        {
            add
            {
                throw new ArgumentException();
            }
            remove
            {
                throw new ArgumentException();
            }
        }

        internal event EventHandler Int
        {
            add
            {
                throw new ArgumentException();
            }
            remove
            {
                throw new ArgumentException();
            }
        }

        private event EventHandler Priv
        {
            add
            {
                throw new ArgumentException();
            }
            remove
            {
                throw new ArgumentException();
            }
        }

        protected event EventHandler Prot
        {
            add
            {
                throw new ArgumentException();
            }
            remove
            {
                throw new ArgumentException();
            }
        }

        protected internal event EventHandler ProtInt
        {
            add
            {
                throw new ArgumentException();
            }
            remove
            {
                throw new ArgumentException();
            }
        }

        private protected event EventHandler PrivProt
        {
            add
            {
                throw new ArgumentException();
            }
            remove
            {
                throw new ArgumentException();
            }
        }

        public event EventHandler Normal;
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Normal = new EventMock<EventHandler>(this, "TestClass", "ITestClass", "Normal", "Normal", Strictness.Lenient);
        }

        public EventMock<EventHandler> Normal { get; }

        event EventHandler ITestClass.Normal { add => Normal.Add(value); remove => Normal.Remove(value); }
    }
}
