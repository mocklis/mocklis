using System;
using Mocklis.Core;
using System.Collections.Generic;

namespace Test
{
    [MocklisClass]
    public class TestClass<T> : IEnumerable<T>
    {
    }
}
