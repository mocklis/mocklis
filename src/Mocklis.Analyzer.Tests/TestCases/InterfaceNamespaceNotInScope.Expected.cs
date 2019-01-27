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
        public TestClass()
        {
            DoStuff = new ActionMethodMock<string>(this, "TestClass", "ITest", "DoStuff", "DoStuff");
        }

        public ActionMethodMock<string> DoStuff { get; }

        void InterfaceyStuff.ITest.DoStuff(string stuff) => DoStuff.Call(stuff);
    }
}
