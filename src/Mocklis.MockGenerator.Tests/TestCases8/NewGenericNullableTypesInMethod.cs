#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass
    {
        void DoStuff<T>(T? p) where T : class;
    }

    [MocklisClass]
    public abstract [PARTIAL] class TestClass : ITestClass
    {
    }
}
