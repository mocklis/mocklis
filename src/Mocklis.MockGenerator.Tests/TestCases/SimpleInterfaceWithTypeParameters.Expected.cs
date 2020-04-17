using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass<T>
    {
        T GetAndSet { get; set; }
    }

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass<T> : ITestClass<T>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            GetAndSet = new PropertyMock<T>(this, "TestClass", "ITestClass", "GetAndSet", "GetAndSet", Strictness.Lenient);
        }

        public PropertyMock<T> GetAndSet { get; }
        T ITestClass<T>.GetAndSet { get => GetAndSet.Value; set => GetAndSet.Value = value; }
    }
}
