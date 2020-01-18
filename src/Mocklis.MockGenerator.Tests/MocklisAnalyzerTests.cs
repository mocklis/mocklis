// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisAnalyzerTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.Tests
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp;
    using Mocklis.MockGenerator.Tests.Helpers;
    using Xunit;
    using Xunit.Abstractions;

    #endregion

    public class MocklisAnalyzerTests : IClassFixture<MocklisClassUpdater>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly MocklisClassUpdater _mocklisClassUpdater;

        public MocklisAnalyzerTests(ITestOutputHelper testOutputHelper, MocklisClassUpdater mocklisClassUpdater)
        {
            _testOutputHelper = testOutputHelper;
            _mocklisClassUpdater = mocklisClassUpdater;
        }

        public static TheoryData<string, string> GetTestCasesFromFolder(string testCaseFolder)
        {
            var pathToTestCases = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), testCaseFolder);

            var data = new TheoryData<string, string>();

            foreach (var file in Directory.EnumerateFiles(pathToTestCases, "*.cs"))
            {
                if (!file.EndsWith(".Expected.cs"))
                {
                    data.Add(testCaseFolder, Path.GetFileNameWithoutExtension(file));
                }
            }

            return data;
        }

        public static TheoryData<string, string> EnumerateTestCases() => GetTestCasesFromFolder("TestCases");

        public static TheoryData<string, string> EnumerateTestCases8() => GetTestCasesFromFolder("TestCases8");

        [Theory]
        [MemberData(nameof(EnumerateTestCases))]
        public async Task TestMocklisClassUpdaterForCSharp7_3(string testCaseFolder, string testCase)
        {
            await TestCodeGenerationCase(testCaseFolder, testCase, LanguageVersion.CSharp7_3);
        }

        [Theory]
        [MemberData(nameof(EnumerateTestCases))]
        [MemberData(nameof(EnumerateTestCases8))]
        public async Task TestMocklisClassUpdaterForCSharp8(string testCaseFolder, string testCase)
        {
            await TestCodeGenerationCase(testCaseFolder, testCase, LanguageVersion.CSharp8);
        }

        private async Task TestCodeGenerationCase(string testCaseFolder, string testCase, LanguageVersion languageVersion)
        {
            var pathToTestCases = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), testCaseFolder);
            string source = File.ReadAllText(Path.Combine(pathToTestCases, testCase + ".cs"));
            string expectedFileName = Path.Combine(pathToTestCases, testCase + ".Expected.cs");
            string? expected = null;
            if (File.Exists(expectedFileName))
            {
                expected = File.ReadAllText(expectedFileName);
            }

            var result = await _mocklisClassUpdater.UpdateMocklisClass(source, languageVersion);

#if !NCRUNCH
            // Create the 'expected' file if it isn't there. Empty out to recreate.
            if (string.IsNullOrWhiteSpace(expected))
            {
                string newPath = Path.Combine(pathToTestCases, "..", "..", "..", "..", testCaseFolder, testCase + ".Expected.cs");
                File.WriteAllText(newPath, result.Code);
                expected = result.Code;
            }
#endif

            try
            {
                Assert.Equal(expected, result.Code);
            }
            catch (Xunit.Sdk.EqualException ex)
            {
                if (!result.IsSuccess)
                {
                    _testOutputHelper.WriteLine(ex.Message);
                    _testOutputHelper.WriteLine(string.Empty);
                }
                else
                {
                    throw;
                }
            }

            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                {
                    _testOutputHelper.WriteLine(error.ErrorText);
                    _testOutputHelper.WriteLine(string.Empty);
                    foreach (var line in error.MarkedCodeLines())
                    {
                        _testOutputHelper.WriteLine(line);
                    }

                    _testOutputHelper.WriteLine(string.Empty);
                }

                throw new Exception("Compilation failed...");
            }
        }
    }
}
