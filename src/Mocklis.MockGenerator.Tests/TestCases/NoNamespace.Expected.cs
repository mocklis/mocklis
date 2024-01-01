using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

public interface ITestClass
{
    int GetOnly { get; }
}

[MocklisClass, GeneratedCode("Mocklis", "[VERSION]")]
public class TestClass : ITestClass
{
    // The contents of this class were created by the Mocklis code-generator.
    // Any changes you make will be overwritten if the contents are re-generated.

    public TestClass()
    {
        GetOnly = new PropertyMock<int>(this, "TestClass", "ITestClass", "GetOnly", "GetOnly", Strictness.Lenient);
    }

    public PropertyMock<int> GetOnly { get; }

    int ITestClass.GetOnly => GetOnly.Value;
}