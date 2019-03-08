using System;
using Mocklis.Core;

namespace Test
{
    public interface ITestClass
    {
        bool TryParse(string source, out int result);
        int Test(in int t1, in int t2);
        void Mutate(ref string value);
    }

    [MocklisClass]
    public class TestClass : ITestClass
    {
    }
}
