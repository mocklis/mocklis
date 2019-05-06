using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void Test(int Item2, int AnotherItem);
        int Test2(out int Item1);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Test = new ActionMethodMock<(int Item2_, int AnotherItem)>(this, "TestClass", "ITestClass", "Test", "Test");
            Test2 = new FuncMethodMock<(int returnValue, int Item1_)>(this, "TestClass", "ITestClass", "Test2", "Test2");
        }

        public ActionMethodMock<(int Item2_, int AnotherItem)> Test { get; }

        void ITestClass.Test(int Item2, int AnotherItem) => Test.Call((Item2, AnotherItem));

        public FuncMethodMock<(int returnValue, int Item1_)> Test2 { get; }

        int ITestClass.Test2(out int Item1)
        {
            var tmp = Test2.Call();
            Item1 = tmp.Item1_;
            return tmp.returnValue;
        }
    }
}
