using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public ref struct RefStruct
    {
        public int Test { get; }

        public RefStruct(int test)
        {
            Test = test;
        }
    }

    public interface ITestClass<TOuter>
    {
        void Test<T>(RefStruct refStruct, TOuter outer, T parameter);
        void TestWithConstraint<T>(RefStruct refStruct, TOuter outer, T parameter) where T : TOuter;
        ref readonly T TestWithRef<T>();
    }

    [MocklisClass]
    public [PARTIAL] class TestClass<T> : ITestClass<T>
    {
    }
}
