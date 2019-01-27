using System;
using Mocklis.Core;
using System.Collections.Generic;

namespace Test
{
    [MocklisClass]
    public class TestClass<T> : IEnumerable<T>
    {
        public TestClass()
        {
            GetEnumerator = new FuncMethodMock<IEnumerator<T>>(this, "TestClass", "IEnumerable", "GetEnumerator", "GetEnumerator");
            GetEnumerator0 = new FuncMethodMock<System.Collections.IEnumerator>(this, "TestClass", "IEnumerable", "GetEnumerator", "GetEnumerator0");
        }

        public FuncMethodMock<IEnumerator<T>> GetEnumerator { get; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator.Call();

        public FuncMethodMock<System.Collections.IEnumerator> GetEnumerator0 { get; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator0.Call();
    }
}
