#nullable disable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections.Generic;

    public interface ITestClass
    {
        string NormalProperty { get; }
        string this[string s] { get; }
        event EventHandler NonNullableEvent;
        string Method3(string p1, string p2);
        int ValueTypeProperty { get; }
        int? NullableValueTypeProperty { get; }
    }

#nullable enable

    [MocklisClass]
    public abstract [PARTIAL] class TestClass : ITestClass
    {
    }
}
