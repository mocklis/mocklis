using System;
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

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
