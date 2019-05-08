using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string Test(int i);
        bool AnotherTest { get; set; }
    }

    public class BaseClass
    {
        event EventHandler Test;
        void AnotherTest<T>(Action<T> test)
        {
        }
    }

    [MocklisClass]
    public class TestClass : BaseClass, ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            Test0 = new FuncMethodMock<int, string>(this, "TestClass", "ITestClass", "Test", "Test0", Strictness.Lenient);
            AnotherTest0 = new PropertyMock<bool>(this, "TestClass", "ITestClass", "AnotherTest", "AnotherTest0", Strictness.Lenient);
        }

        public FuncMethodMock<int, string> Test0 { get; }

        string ITestClass.Test(int i) => Test0.Call(i);

        public PropertyMock<bool> AnotherTest0 { get; }
        bool ITestClass.AnotherTest { get => AnotherTest0.Value; set => AnotherTest0.Value = value; }
    }
}
