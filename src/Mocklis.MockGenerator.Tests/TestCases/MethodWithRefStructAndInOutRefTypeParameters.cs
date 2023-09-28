using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public ref struct RefStruct
    {
        public int Test { get; set;}
    }

    public interface ITestClass
    {
        void RefStructIn<T>(RefStruct refStruct, in T parameter);
        void RefStructOut<T>(RefStruct refStruct, out T paramater);
        void RefStructRef<T>(RefStruct refStruct, ref T parameter);
    }

    [MocklisClass]
    public [PARTIAL] class TestClass : ITestClass
    {
    }
}
