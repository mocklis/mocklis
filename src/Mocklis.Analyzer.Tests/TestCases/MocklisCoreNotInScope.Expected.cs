using System;

namespace Test
{
    public interface ITest
    {
        void DoStuff(string stuff);
    }

    [Mocklis.Core.MocklisClass]
    public class TestClass : ITest
    {
        public TestClass()
        {
            DoStuff = new Mocklis.Core.ActionMethodMock<string>(this, "TestClass", "ITest", "DoStuff", "DoStuff");
        }

        public Mocklis.Core.ActionMethodMock<string> DoStuff { get; }

        void ITest.DoStuff(string stuff) => DoStuff.Call(stuff);
    }
}
