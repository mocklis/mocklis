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
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            DoStuff = new Mocklis.Core.ActionMethodMock<string>(this, "TestClass", "ITest", "DoStuff", "DoStuff", Mocklis.Core.Strictness.Lenient);
        }

        public Mocklis.Core.ActionMethodMock<string> DoStuff { get; }

        void ITest.DoStuff(string stuff) => DoStuff.Call(stuff);
    }
}
