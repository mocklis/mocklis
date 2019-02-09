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
    }
}
