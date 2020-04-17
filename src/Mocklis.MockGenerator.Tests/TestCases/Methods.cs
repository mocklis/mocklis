using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void ActionWithoutParameters();
        void ActionWithOneParameter(int i);
        void ActionWithTwoParameters(int i1, int i2);
        void ActionWithThreeParameters(int i1, int i2, int i3);
        int FuncWithoutParameters();
        int FuncWithOneParameter(int i);
        int FuncWithTwoParameters(int i1, int i2);
        int FuncWithThreeParameters(int i1, int i2, int i3);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
