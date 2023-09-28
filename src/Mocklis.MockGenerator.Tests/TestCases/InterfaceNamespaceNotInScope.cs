using System;
using System.CodeDom.Compiler;
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
    public [PARTIAL] class TestClass : InterfaceyStuff.ITest
    {
    }
}
