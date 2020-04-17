using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITest1
    {
        int Property1 { get; set;}
    }

    public interface ITest2
    {
        void DoSomething(string task);
    }

    [MocklisClass]
    public class TestClass : ITest1, ITest2
    {
    }
}
