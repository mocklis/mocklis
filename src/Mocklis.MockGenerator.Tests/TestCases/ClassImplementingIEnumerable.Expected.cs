using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Mocklis.Core;

namespace Test
{
    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass<T> : IEnumerable<T>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            GetEnumerator = new FuncMethodMock<IEnumerator<T>>(this, "TestClass", "IEnumerable", "GetEnumerator", "GetEnumerator", Strictness.Lenient);
            GetEnumerator0 = new FuncMethodMock<System.Collections.IEnumerator>(this, "TestClass", "IEnumerable", "GetEnumerator", "GetEnumerator0", Strictness.Lenient);
        }

        public FuncMethodMock<IEnumerator<T>> GetEnumerator { get; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator.Call();

        public FuncMethodMock<System.Collections.IEnumerator> GetEnumerator0 { get; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator0.Call();
    }
}
