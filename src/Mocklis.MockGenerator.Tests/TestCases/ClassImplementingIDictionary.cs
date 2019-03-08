using System;
using Mocklis.Core;
using System.Collections.Generic;

namespace Test
{
    [MocklisClass]
    public class TestClass<TKey, TValue> : IDictionary<TKey, TValue>
    {
    }
}
