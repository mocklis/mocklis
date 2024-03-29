// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisAnalyzerTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
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
    using Xunit.Sdk;

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


        [Theory]
        [MemberData(nameof(TestCaseEnumerator.EnumerateTestCases), MemberType = typeof(TestCaseEnumerator))]
        public async Task TestMocklisClassUpdaterForCSharp7_3(ClassUpdateTestCase test)
        {
            await TestCodeGenerationCase(test, LanguageVersion.CSharp7_3);
        }

        [Theory]
        [MemberData(nameof(TestCaseEnumerator.EnumerateTestCases), MemberType = typeof(TestCaseEnumerator))]
        [MemberData(nameof(TestCaseEnumerator.EnumerateTestCases8), MemberType = typeof(TestCaseEnumerator))]
        public async Task TestMocklisClassUpdaterForCSharp8(ClassUpdateTestCase test)
        {
            await TestCodeGenerationCase(test, LanguageVersion.CSharp8);
        }

        private async Task TestCodeGenerationCase(ClassUpdateTestCase test, LanguageVersion languageVersion)
        {
            Task<string> sourceTask = File.ReadAllTextAsync(test.SourceFileName);

            Task<string> expectedTask = Task.FromResult(string.Empty);
            if (File.Exists(test.ExpectedFileName))
            {
                expectedTask = File.ReadAllTextAsync(test.ExpectedFileName);
            }

            string source = await sourceTask.ConfigureAwait(false);

            source = source.Replace("[PARTIAL] ", "");

            var result = await _mocklisClassUpdater.UpdateMocklisClass(source, languageVersion).ConfigureAwait(false);

            string expected = (await expectedTask.ConfigureAwait(false)).Replace("[VERSION]", _version);

            // Create the 'expected' file if it isn't there. Empty out to recreate.
            if (string.IsNullOrWhiteSpace(expected))
            {
#if NCRUNCH
                var folder = Environment.GetEnvironmentVariable("MockGeneratorTestsFolder");
                string expectedFilePathInSourceCode =
                    folder == null ? string.Empty : Path.Combine(folder, test.TestCaseFolder, test.TestCase + ".Expected.cs");
#else
                string expectedFilePathInSourceCode =
                    Path.Combine(test.PathToTestCases, "..", "..", "..", test.TestCaseFolder, test.TestCase + ".Expected.cs");
#endif
                if (!string.IsNullOrWhiteSpace(expectedFilePathInSourceCode))
                {
                    await File.WriteAllTextAsync(expectedFilePathInSourceCode, result.Code.Replace(_version, "[VERSION]")).ConfigureAwait(false);
                }

                expected = result.Code;
            }

            int i = 0;

            try
            {
                var e = expected.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                var c = result.Code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                var linesToCheck = Math.Max(e.Length, c.Length);
                for (i = 0; i < linesToCheck; i++)
                {
                    var eline = i <= e.Length ? e[i] : string.Empty;
                    var cline = i <= c.Length ? c[i] : string.Empty;
                    Assert.Equal(eline, cline);
                }
            }
            catch (EqualException ex)
            {
                if (!result.IsSuccess)
                {
                    _testOutputHelper.WriteLine($"Mismatch on line {i + 1}:");
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
