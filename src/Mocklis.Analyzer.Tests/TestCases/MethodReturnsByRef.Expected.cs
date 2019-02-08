using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        ref int ReturnsByRef();
        ref readonly int ReturnsByRefReadonly();
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
        public TestClass()
        {
            ReturnsByRefReadonly = new FuncMethodMock<int>(this, "TestClass", "ITestClass", "ReturnsByRefReadonly", "ReturnsByRefReadonly");
        }

        protected virtual ref int ReturnsByRef()
        {
            throw new MockMissingException(MockType.VirtualMethod, "TestClass", "ITestClass", "ReturnsByRef", "ReturnsByRef");
        }

        ref int ITestClass.ReturnsByRef() => ref ReturnsByRef();

        public FuncMethodMock<int> ReturnsByRefReadonly { get; }

        ref readonly int ITestClass.ReturnsByRefReadonly() => ref ByRef<int>.Wrap(ReturnsByRefReadonly.Call());
    }
}
