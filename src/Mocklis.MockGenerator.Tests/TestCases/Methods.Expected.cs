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

    [MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
    public class TestClass : ITestClass
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public TestClass()
        {
            ActionWithoutParameters = new ActionMethodMock(this, "TestClass", "ITestClass", "ActionWithoutParameters", "ActionWithoutParameters", Strictness.Lenient);
            ActionWithOneParameter = new ActionMethodMock<int>(this, "TestClass", "ITestClass", "ActionWithOneParameter", "ActionWithOneParameter", Strictness.Lenient);
            ActionWithTwoParameters = new ActionMethodMock<(int i1, int i2)>(this, "TestClass", "ITestClass", "ActionWithTwoParameters", "ActionWithTwoParameters", Strictness.Lenient);
            ActionWithThreeParameters = new ActionMethodMock<(int i1, int i2, int i3)>(this, "TestClass", "ITestClass", "ActionWithThreeParameters", "ActionWithThreeParameters", Strictness.Lenient);
            FuncWithoutParameters = new FuncMethodMock<int>(this, "TestClass", "ITestClass", "FuncWithoutParameters", "FuncWithoutParameters", Strictness.Lenient);
            FuncWithOneParameter = new FuncMethodMock<int, int>(this, "TestClass", "ITestClass", "FuncWithOneParameter", "FuncWithOneParameter", Strictness.Lenient);
            FuncWithTwoParameters = new FuncMethodMock<(int i1, int i2), int>(this, "TestClass", "ITestClass", "FuncWithTwoParameters", "FuncWithTwoParameters", Strictness.Lenient);
            FuncWithThreeParameters = new FuncMethodMock<(int i1, int i2, int i3), int>(this, "TestClass", "ITestClass", "FuncWithThreeParameters", "FuncWithThreeParameters", Strictness.Lenient);
        }

        public ActionMethodMock ActionWithoutParameters { get; }

        void ITestClass.ActionWithoutParameters() => ActionWithoutParameters.Call();

        public ActionMethodMock<int> ActionWithOneParameter { get; }

        void ITestClass.ActionWithOneParameter(int i) => ActionWithOneParameter.Call(i);

        public ActionMethodMock<(int i1, int i2)> ActionWithTwoParameters { get; }

        void ITestClass.ActionWithTwoParameters(int i1, int i2) => ActionWithTwoParameters.Call((i1, i2));

        public ActionMethodMock<(int i1, int i2, int i3)> ActionWithThreeParameters { get; }

        void ITestClass.ActionWithThreeParameters(int i1, int i2, int i3) => ActionWithThreeParameters.Call((i1, i2, i3));

        public FuncMethodMock<int> FuncWithoutParameters { get; }

        int ITestClass.FuncWithoutParameters() => FuncWithoutParameters.Call();

        public FuncMethodMock<int, int> FuncWithOneParameter { get; }

        int ITestClass.FuncWithOneParameter(int i) => FuncWithOneParameter.Call(i);

        public FuncMethodMock<(int i1, int i2), int> FuncWithTwoParameters { get; }

        int ITestClass.FuncWithTwoParameters(int i1, int i2) => FuncWithTwoParameters.Call((i1, i2));

        public FuncMethodMock<(int i1, int i2, int i3), int> FuncWithThreeParameters { get; }

        int ITestClass.FuncWithThreeParameters(int i1, int i2, int i3) => FuncWithThreeParameters.Call((i1, i2, i3));
    }
}
