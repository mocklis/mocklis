using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void Item2_(int Item2, int anotherItem);
        int Item3_(ref string Item3);
        int Item4_<T>(ref T Item4);
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Item2_ = new ActionMethodMock<(int Item2_, int anotherItem)>(this, "TestClass", "ITestClass", "Item2_", "Item2_", Strictness.Lenient);
            Item3_ = new FuncMethodMock<string, (int returnValue, string Item3_)>(this, "TestClass", "ITestClass", "Item3_", "Item3_", Strictness.Lenient);
        }

        public ActionMethodMock<(int Item2_, int anotherItem)> Item2_ { get; }

        void ITestClass.Item2_(int Item2, int anotherItem) => Item2_.Call((Item2, anotherItem));

        public FuncMethodMock<string, (int returnValue, string Item3_)> Item3_ { get; }

        int ITestClass.Item3_(ref string Item3)
        {
            var tmp = Item3_.Call(Item3);
            Item3 = tmp.Item3_;
            return tmp.returnValue;
        }

        private readonly TypedMockProvider _item4_ = new TypedMockProvider();

        public FuncMethodMock<T, (int returnValue, T Item4_)> Item4_<T>()
        {
            var key = new[] { typeof(T) };
            return (FuncMethodMock<T, (int returnValue, T Item4_)>)_item4_.GetOrAdd(key, keyString => new FuncMethodMock<T, (int returnValue, T Item4_)>(this, "TestClass", "ITestClass", "Item4_" + keyString, "Item4_" + keyString + "()", Strictness.Lenient));
        }

        int ITestClass.Item4_<T>(ref T Item4)
        {
            var tmp = Item4_<T>().Call(Item4);
            Item4 = tmp.Item4_;
            return tmp.returnValue;
        }
    }
}
