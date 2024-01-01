#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        string? NullableProperty { get; }
        string NormalProperty { get; }
        string? this[int i] { get; }
        string this[bool b] { get; }
        string? this[string? s] { get; }
        string this[int i, string? s] { get; }
        event EventHandler NonNullableEvent;
        event EventHandler? NullableEvent;
        string Method1(string? parameter);
        string? Method2(string parameter);
        string Method3(string p1, string? p2);
    }

#nullable disable

    [MocklisClass]
    public abstract [PARTIAL] class TestClass : ITestClass
    {
    }
}
