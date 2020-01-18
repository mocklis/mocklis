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
    }
}
