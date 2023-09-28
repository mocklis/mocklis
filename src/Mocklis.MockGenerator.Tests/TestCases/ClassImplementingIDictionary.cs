using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Mocklis.Core;

namespace Test
{
    [MocklisClass]
    public [PARTIAL] class TestClass<TKey, TValue> : IDictionary<TKey, TValue>
    {
    }
}
