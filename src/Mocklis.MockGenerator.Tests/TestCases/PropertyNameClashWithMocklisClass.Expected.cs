using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int TestClass { get; set; }
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            TestClass0 = new PropertyMock<int>(this, "TestClass", "ITestClass", "TestClass", "TestClass0", Strictness.Lenient);
        }

        public PropertyMock<int> TestClass0 { get; }
        int ITestClass.TestClass { get => TestClass0.Value; set => TestClass0.Value = value; }
    }
}
