using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef();
        ref readonly int ReturnsByRefReadonly();
        ref readonly int ReturnsMoreStuffByRef(out int blah);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            ReturnsByRefReadonly = new FuncMethodMock<int>(this, "TestClass", "ITestClass", "ReturnsByRefReadonly", "ReturnsByRefReadonly");
            ReturnsMoreStuffByRef = new FuncMethodMock<(int returnValue, int blah)>(this, "TestClass", "ITestClass", "ReturnsMoreStuffByRef", "ReturnsMoreStuffByRef");
        }

        protected virtual ref int ReturnsByRef()
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "ReturnsByRef", "ReturnsByRef");
        }

        ref int ITestClass.ReturnsByRef() => ref ReturnsByRef();

        public FuncMethodMock<int> ReturnsByRefReadonly { get; }

        ref readonly int ITestClass.ReturnsByRefReadonly() => ref ByRef<int>.Wrap(ReturnsByRefReadonly.Call());

        public FuncMethodMock<(int returnValue, int blah)> ReturnsMoreStuffByRef { get; }

        ref readonly int ITestClass.ReturnsMoreStuffByRef(out int blah)
        {
            var tmp = ReturnsMoreStuffByRef.Call();
            blah = tmp.blah;
            return ref ByRef<int>.Wrap(tmp.returnValue);
        }
    }
}
