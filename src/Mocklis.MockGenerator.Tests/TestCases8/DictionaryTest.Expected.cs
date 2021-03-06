#nullable enable
using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    using System.Collections.Generic;

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public abstract class TestClass<TKey, TValue> : IDictionary<TKey, TValue>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        protected TestClass()
        {
            ContainsKey = new FuncMethodMock<TKey, bool>(this, "TestClass", "IDictionary", "ContainsKey", "ContainsKey", Strictness.Lenient);
            Add = new ActionMethodMock<(TKey key, TValue value)>(this, "TestClass", "IDictionary", "Add", "Add", Strictness.Lenient);
            Remove = new FuncMethodMock<TKey, bool>(this, "TestClass", "IDictionary", "Remove", "Remove", Strictness.Lenient);
            TryGetValue = new FuncMethodMock<TKey, (bool returnValue, TValue value)>(this, "TestClass", "IDictionary", "TryGetValue", "TryGetValue", Strictness.Lenient);
            Item = new IndexerMock<TKey, TValue>(this, "TestClass", "IDictionary", "this[]", "Item", Strictness.Lenient);
            Keys = new PropertyMock<ICollection<TKey>>(this, "TestClass", "IDictionary", "Keys", "Keys", Strictness.Lenient);
            Values = new PropertyMock<ICollection<TValue>>(this, "TestClass", "IDictionary", "Values", "Values", Strictness.Lenient);
            Add0 = new ActionMethodMock<KeyValuePair<TKey, TValue>>(this, "TestClass", "ICollection", "Add", "Add0", Strictness.Lenient);
            Clear = new ActionMethodMock(this, "TestClass", "ICollection", "Clear", "Clear", Strictness.Lenient);
            Contains = new FuncMethodMock<KeyValuePair<TKey, TValue>, bool>(this, "TestClass", "ICollection", "Contains", "Contains", Strictness.Lenient);
            CopyTo = new ActionMethodMock<(KeyValuePair<TKey, TValue>[] array, int arrayIndex)>(this, "TestClass", "ICollection", "CopyTo", "CopyTo", Strictness.Lenient);
            Remove0 = new FuncMethodMock<KeyValuePair<TKey, TValue>, bool>(this, "TestClass", "ICollection", "Remove", "Remove0", Strictness.Lenient);
            Count = new PropertyMock<int>(this, "TestClass", "ICollection", "Count", "Count", Strictness.Lenient);
            IsReadOnly = new PropertyMock<bool>(this, "TestClass", "ICollection", "IsReadOnly", "IsReadOnly", Strictness.Lenient);
            GetEnumerator = new FuncMethodMock<IEnumerator<KeyValuePair<TKey, TValue>>>(this, "TestClass", "IEnumerable", "GetEnumerator", "GetEnumerator", Strictness.Lenient);
            GetEnumerator0 = new FuncMethodMock<System.Collections.IEnumerator>(this, "TestClass", "IEnumerable", "GetEnumerator", "GetEnumerator0", Strictness.Lenient);
        }

        public FuncMethodMock<TKey, bool> ContainsKey { get; }

        bool IDictionary<TKey, TValue>.ContainsKey(TKey key) => ContainsKey.Call(key);

        public ActionMethodMock<(TKey key, TValue value)> Add { get; }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => Add.Call((key, value));

        public FuncMethodMock<TKey, bool> Remove { get; }

        bool IDictionary<TKey, TValue>.Remove(TKey key) => Remove.Call(key);

        public FuncMethodMock<TKey, (bool returnValue, TValue value)> TryGetValue { get; }

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            var tmp = TryGetValue.Call(key);
            value = tmp.value;
            return tmp.returnValue;
        }

        public IndexerMock<TKey, TValue> Item { get; }

        TValue IDictionary<TKey, TValue>.this[TKey key] { get => Item[key]; set => Item[key] = value; }

        public PropertyMock<ICollection<TKey>> Keys { get; }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys.Value;

        public PropertyMock<ICollection<TValue>> Values { get; }

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values.Value;

        public ActionMethodMock<KeyValuePair<TKey, TValue>> Add0 { get; }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add0.Call(item);

        public ActionMethodMock Clear { get; }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => Clear.Call();

        public FuncMethodMock<KeyValuePair<TKey, TValue>, bool> Contains { get; }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => Contains.Call(item);

        public ActionMethodMock<(KeyValuePair<TKey, TValue>[] array, int arrayIndex)> CopyTo { get; }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => CopyTo.Call((array, arrayIndex));

        public FuncMethodMock<KeyValuePair<TKey, TValue>, bool> Remove0 { get; }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => Remove0.Call(item);

        public PropertyMock<int> Count { get; }

        int ICollection<KeyValuePair<TKey, TValue>>.Count => Count.Value;

        public PropertyMock<bool> IsReadOnly { get; }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => IsReadOnly.Value;

        public FuncMethodMock<IEnumerator<KeyValuePair<TKey, TValue>>> GetEnumerator { get; }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator.Call();

        public FuncMethodMock<System.Collections.IEnumerator> GetEnumerator0 { get; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator0.Call();
    }
}
