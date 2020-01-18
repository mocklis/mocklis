#nullable enable
using System;
using Mocklis.Core;

namespace Test
{
    using System.Collections.Generic;

    [MocklisClass]
    public abstract class TestClass<TKey, TValue> : IDictionary<TKey, TValue>
    {
    }
}
