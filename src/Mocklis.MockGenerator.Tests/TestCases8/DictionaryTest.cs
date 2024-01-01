#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections.Generic;

    [MocklisClass]
    public abstract [PARTIAL] class TestClass<TKey, TValue> : IDictionary<TKey, TValue>
    {
    }
}
