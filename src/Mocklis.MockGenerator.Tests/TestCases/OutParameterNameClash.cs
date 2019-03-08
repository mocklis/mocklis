using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        bool TryParse1(string tmp, out int result);
        bool TryParse2(string source, out int tmp);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
