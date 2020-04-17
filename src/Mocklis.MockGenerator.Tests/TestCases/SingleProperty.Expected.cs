using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        int GetAndSet { get; set; }
        int SetOnly { set; }
        int GetOnly { get; }
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            GetAndSet = new PropertyMock<int>(this, "TestClass", "ITestClass", "GetAndSet", "GetAndSet", Strictness.Lenient);
            SetOnly = new PropertyMock<int>(this, "TestClass", "ITestClass", "SetOnly", "SetOnly", Strictness.Lenient);
            GetOnly = new PropertyMock<int>(this, "TestClass", "ITestClass", "GetOnly", "GetOnly", Strictness.Lenient);
        }

        public PropertyMock<int> GetAndSet { get; }
        int ITestClass.GetAndSet { get => GetAndSet.Value; set => GetAndSet.Value = value; }
        public PropertyMock<int> SetOnly { get; }
        int ITestClass.SetOnly { set => SetOnly.Value = value; }
        public PropertyMock<int> GetOnly { get; }

        int ITestClass.GetOnly => GetOnly.Value;
    }
}
