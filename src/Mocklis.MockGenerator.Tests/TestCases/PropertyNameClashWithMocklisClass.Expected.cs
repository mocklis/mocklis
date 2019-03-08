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
        public TestClass()
        {
            TestClass0 = new PropertyMock<int>(this, "TestClass", "ITestClass", "TestClass", "TestClass0");
        }

        public PropertyMock<int> TestClass0 { get; }
        int ITestClass.TestClass { get => TestClass0.Value; set => TestClass0.Value = value; }
    }
}
