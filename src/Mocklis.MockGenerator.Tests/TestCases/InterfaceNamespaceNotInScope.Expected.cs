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
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            DoStuff = new ActionMethodMock<string>(this, "TestClass", "ITest", "DoStuff", "DoStuff", Strictness.Lenient);
        }

        public ActionMethodMock<string> DoStuff { get; }

        void InterfaceyStuff.ITest.DoStuff(string stuff) => DoStuff.Call(stuff);
    }
}
