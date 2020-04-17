#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections;
    using System.Collections.Generic;

    public interface ITestClass<T> where T : notnull
    {
        T Test<U>(U item) where U : notnull;
    }

#nullable disable
    [MocklisClass]
    public abstract class TestClass<T> : ITestClass<T>
    {
    }
}
