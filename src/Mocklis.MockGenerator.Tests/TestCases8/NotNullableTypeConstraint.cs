#nullable enable
using System;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass<T> where T : notnull
    {
        T Test<U>(U item) where U : notnull;
    }

    [MocklisClass]
    public abstract class TestClass<T> : ITestClass<T> where T : notnull
    {
    }
}
