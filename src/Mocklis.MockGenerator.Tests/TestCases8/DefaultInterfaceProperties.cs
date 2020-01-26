using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        static int _field;

        public int PubGet => 0;

        public int PubSet
        {
            set => _field = value;
        }

        public int Pub
        {
            get => 0;
            set => _field = value;
        }
        
        static int StatGet => 0;

        static int StatSet
        {
            set => _field = value;
        }

        static int Stat
        {
            get => 0;
            set => _field = value;
        }

        internal int IntGet => 0;

        internal int IntSet
        {
            set => _field = value;
        }

        internal int Int
        {
            get => 0;
            set => _field = value;
        }

        private int PrivGet => 0;

        private int PrivSet
        {
            set => _field = value;
        }

        private int Priv
        {
            get => 0;
            set => _field = value;
        }

        protected int ProtGet => 0;

        protected int ProtSet
        {
            set => _field = value;
        }

        protected int Prot
        {
            get => 0;
            set => _field = value;
        }

        protected internal int ProtIntGet => 0;

        protected internal int ProtIntSet
        {
            set => _field = value;
        }

        protected internal int ProtInt
        {
            get => 0;
            set => _field = value;
        }

        private protected int PrivProtGet => 0;

        private protected int PrivProtSet
        {
            set => _field = value;
        }

        private protected int PrivProt
        {
            get => 0;
            set => _field = value;
        }

        public int Normal { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
