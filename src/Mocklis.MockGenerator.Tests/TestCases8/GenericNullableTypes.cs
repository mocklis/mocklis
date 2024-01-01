#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass<T> where T : class
    {
        T? Property { get; set; }
        T? this[T? p1, T? p2] { get; set; }
        T? Test(T? item);
    }

    [MocklisClass]
    public abstract [PARTIAL] class TestClass<T> : ITestClass<T> where T : class
    {
    }
}
