using System;
using System.CodeDom.Compiler;
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
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
