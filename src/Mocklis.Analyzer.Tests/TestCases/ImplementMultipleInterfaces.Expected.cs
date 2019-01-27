using System;
using Mocklis.Core;

namespace Test
{
    public interface ITest1
    {
        int Property1 { get; set;}
    }

    public interface ITest2
    {
        void DoSomething(string task);
    }

    [MocklisClass]
    public class TestClass : ITest1, ITest2
    {
        public TestClass()
        {
            Property1 = new PropertyMock<int>(this, "TestClass", "ITest1", "Property1", "Property1");
            DoSomething = new ActionMethodMock<string>(this, "TestClass", "ITest2", "DoSomething", "DoSomething");
        }

        public PropertyMock<int> Property1 { get; }
        int ITest1.Property1 { get => Property1.Value; set => Property1.Value = value; }
        public ActionMethodMock<string> DoSomething { get; }

        void ITest2.DoSomething(string task) => DoSomething.Call(task);
    }
}
