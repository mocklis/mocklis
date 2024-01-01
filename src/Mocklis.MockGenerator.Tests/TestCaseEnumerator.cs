// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCaseEnumerator.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator;

#region Using Directives

using System;
using System.IO;
using System.Reflection;
using Xunit;

#endregion

public static class TestCaseEnumerator
{
    public static TheoryData<ClassUpdateTestCase> GetTestCasesFromFolder(string testCaseFolder)
    {
        var pathToTestCases = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                              throw new InvalidOperationException("Could not find executing assembly folder");

        var result = new TheoryData<ClassUpdateTestCase>();

        foreach (var file in Directory.EnumerateFiles(Path.Combine(pathToTestCases, testCaseFolder), "*.cs"))
        {
            if (!file.EndsWith(".Expected.cs") && !file.EndsWith(".ExpectedSource.cs"))
            {
                result.Add(new ClassUpdateTestCase
                {
                    PathToTestCases = pathToTestCases,
                    TestCaseFolder = testCaseFolder,
                    TestCase = Path.GetFileNameWithoutExtension(file)
                });
            }
        }

        return result;
    }

    public static TheoryData<ClassUpdateTestCase> EnumerateTestCases() => GetTestCasesFromFolder("TestCases");

    public static TheoryData<ClassUpdateTestCase> EnumerateTestCases8() => GetTestCasesFromFolder("TestCases8");
}
