#nullable enable
using System;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass
    {
        event EventHandler NonNullableEvent;
        event EventHandler? NullableEvent;
    }

    [MocklisClass]
    public abstract class TestClass : ITestClass
    {
    }
}
