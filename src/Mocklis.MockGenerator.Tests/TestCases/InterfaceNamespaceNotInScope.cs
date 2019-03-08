using System;
using Mocklis.Core;

namespace InterfaceyStuff
{
    public interface ITest
    {
        void DoStuff(string stuff);
    }
}

namespace Test
{
    [MocklisClass]
    public class TestClass : InterfaceyStuff.ITest
    {
    }
}
