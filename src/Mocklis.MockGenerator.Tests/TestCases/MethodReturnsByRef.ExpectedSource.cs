// <auto-generated />

namespace Test
{
    partial class TestClass
    {
        protected virtual ref int ReturnsByRef()
        {
            throw new global::Mocklis.Core.MockMissingException(global::Mocklis.Core.MockType.VirtualMethod, "TestClass", "ITestClass", "ReturnsByRef", "ReturnsByRef");
        }

        ref int global::Test.ITestClass.ReturnsByRef() => ref ReturnsByRef();

        public global::Mocklis.Core.FuncMethodMock<int> ReturnsByRefReadonly { get; }

        ref readonly int global::Test.ITestClass.ReturnsByRefReadonly() => ref global::Mocklis.Core.ByRef<int>.Wrap(ReturnsByRefReadonly.Call());

        public global::Mocklis.Core.FuncMethodMock<(int returnValue, int blah)> ReturnsMoreStuffByRef { get; }

        ref readonly int global::Test.ITestClass.ReturnsMoreStuffByRef(out int blah)
        {
            var tmp = ReturnsMoreStuffByRef.Call();
            blah = tmp.blah;
            return ref global::Mocklis.Core.ByRef<int>.Wrap(tmp.returnValue);
        }

        public TestClass() : base()
        {
            this.ReturnsByRefReadonly = new global::Mocklis.Core.FuncMethodMock<int>(this, "TestClass", "ITestClass", "ReturnsByRefReadonly", "ReturnsByRefReadonly", global::Mocklis.Core.Strictness.Lenient);
            this.ReturnsMoreStuffByRef = new global::Mocklis.Core.FuncMethodMock<(int returnValue, int blah)>(this, "TestClass", "ITestClass", "ReturnsMoreStuffByRef", "ReturnsMoreStuffByRef", global::Mocklis.Core.Strictness.Lenient);
        }
    }
}
