#nullable enable
using System;
using Mocklis.Core;

namespace Test
{
    using System.Collections.Generic;

    public interface ITestClass
    {
        IDictionary<int?, List<(byte, string?)>?> Test { get; }
    }

    [MocklisClass]
    public abstract class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected TestClass()
        {
            Test = new PropertyMock<IDictionary<int?, List<(byte, string?)>?>>(this, "TestClass", "ITestClass", "Test", "Test", Strictness.Lenient);
        }

        public PropertyMock<IDictionary<int?, List<(byte, string?)>?>> Test { get; }

        IDictionary<int?, List<(byte, string?)>?> ITestClass.Test => Test.Value;
    }
}
