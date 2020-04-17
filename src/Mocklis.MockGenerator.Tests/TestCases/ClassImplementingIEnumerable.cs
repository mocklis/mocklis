using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Mocklis.Core;

namespace Test
{
    [MocklisClass]
    public class TestClass<T> : IEnumerable<T>
    {
    }
}
