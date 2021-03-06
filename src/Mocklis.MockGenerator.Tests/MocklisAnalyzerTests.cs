// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisAnalyzerTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp;
    using Mocklis.MockGenerator.Helpers;
    using Xunit;
    using Xunit.Abstractions;

    #endregion

    public class MocklisAnalyzerTests : IClassFixture<MocklisClassUpdater>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly MocklisClassUpdater _mocklisClassUpdater;
        private readonly string _version;

        public MocklisAnalyzerTests(ITestOutputHelper testOutputHelper, MocklisClassUpdater mocklisClassUpdater)
        {
            _testOutputHelper = testOutputHelper;
            _mocklisClassUpdater = mocklisClassUpdater;
            _version = Assembly.GetAssembly(typeof(MocklisAnalyzer))
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? string.Empty;
        }

        public static TheoryData<ClassUpdateTestCase> GetTestCasesFromFolder(string testCaseFolder)
        {
            var pathToTestCases = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                  throw new InvalidOperationException("Could not find executing assembly folder");

            var result = new TheoryData<ClassUpdateTestCase>();

            foreach (var file in Directory.EnumerateFiles(Path.Combine(pathToTestCases, testCaseFolder), "*.cs"))
            {
                if (!file.EndsWith(".Expected.cs"))
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

        [Theory]
        [MemberData(nameof(EnumerateTestCases))]
        public async Task TestMocklisClassUpdaterForCSharp7_3(ClassUpdateTestCase test)
        {
            await TestCodeGenerationCase(test, LanguageVersion.CSharp7_3);
        }

        [Theory]
        [MemberData(nameof(EnumerateTestCases))]
        [MemberData(nameof(EnumerateTestCases8))]
        public async Task TestMocklisClassUpdaterForCSharp8(ClassUpdateTestCase test)
        {
            await TestCodeGenerationCase(test, LanguageVersion.CSharp8);
        }

        private async Task TestCodeGenerationCase(ClassUpdateTestCase test, LanguageVersion languageVersion)
        {
            Task<string> sourceTask = File.ReadAllTextAsync(test.SourceFileName);

            Task<string?> expectedTask = Task.FromResult<string?>(null);
            if (File.Exists(test.ExpectedFileName))
            {
                expectedTask = File.ReadAllTextAsync(test.ExpectedFileName);
            }

            string source = await sourceTask.ConfigureAwait(false);

            var result = await _mocklisClassUpdater.UpdateMocklisClass(source, languageVersion).ConfigureAwait(false);

            string? expected = (await expectedTask.ConfigureAwait(false))?.Replace("[VERSION]", _version);

            // Create the 'expected' file if it isn't there. Empty out to recreate.
            if (string.IsNullOrWhiteSpace(expected))
            {
#if NCRUNCH
                var folder = Environment.GetEnvironmentVariable("MockGeneratorTestsFolder");
                string expectedFilePathInSourceCode =
 folder == null ? null : Path.Combine(folder, test.TestCaseFolder, test.TestCase + ".Expected.cs");
#else
                string expectedFilePathInSourceCode =
                    Path.Combine(test.PathToTestCases, "..", "..", "..", test.TestCaseFolder, test.TestCase + ".Expected.cs");
#endif

                if (expectedFilePathInSourceCode != null)
                {
                    await File.WriteAllTextAsync(expectedFilePathInSourceCode, result.Code.Replace(_version, "[VERSION]")).ConfigureAwait(false);
                    expected = result.Code;
                }
            }

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
