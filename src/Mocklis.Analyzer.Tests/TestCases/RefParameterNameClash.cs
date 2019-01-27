using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        void UpdateScore(int tmp, ref int score);
        void UpdateScore(string action, ref int tmp);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
