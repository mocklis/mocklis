using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void Restricted(RuntimeArgumentHandle runtimeArgumentHandle);
        void Restricted(ArgIterator argIterator);
        void Restricted(TypedReference typedReference);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected virtual void Restricted(RuntimeArgumentHandle runtimeArgumentHandle)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "Restricted", "Restricted");
        }

        void ITestClass.Restricted(RuntimeArgumentHandle runtimeArgumentHandle) => Restricted(runtimeArgumentHandle);

        protected virtual void Restricted0(ArgIterator argIterator)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "Restricted", "Restricted0");
        }

        void ITestClass.Restricted(ArgIterator argIterator) => Restricted0(argIterator);

        protected virtual void Restricted1(TypedReference typedReference)
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "Restricted", "Restricted1");
        }

        void ITestClass.Restricted(TypedReference typedReference) => Restricted1(typedReference);
    }
}
