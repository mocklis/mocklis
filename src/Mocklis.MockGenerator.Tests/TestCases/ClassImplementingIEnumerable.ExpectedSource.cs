// <auto-generated />

namespace Test
{
    partial class TestClass<T>
    {
        public global::Mocklis.Core.FuncMethodMock<global::System.Collections.Generic.IEnumerator<T>> GetEnumerator { get; }

        global::System.Collections.Generic.IEnumerator<T> global::System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator.Call();

        public global::Mocklis.Core.FuncMethodMock<global::System.Collections.IEnumerator> GetEnumerator0 { get; }

        global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() => GetEnumerator0.Call();

        public TestClass() : base()
        {
            this.GetEnumerator = new global::Mocklis.Core.FuncMethodMock<global::System.Collections.Generic.IEnumerator<T>>(this, "TestClass", "IEnumerable", "GetEnumerator", "GetEnumerator", global::Mocklis.Core.Strictness.Lenient);
            this.GetEnumerator0 = new global::Mocklis.Core.FuncMethodMock<global::System.Collections.IEnumerator>(this, "TestClass", "IEnumerable", "GetEnumerator", "GetEnumerator0", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
