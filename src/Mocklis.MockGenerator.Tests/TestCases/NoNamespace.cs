using System;
using System.CodeDom.Compiler;
using Mocklis.Core;

public interface ITestClass
{
    int GetOnly { get; }
}

[MocklisClass]
public [PARTIAL] class TestClass : ITestClass
{
}
