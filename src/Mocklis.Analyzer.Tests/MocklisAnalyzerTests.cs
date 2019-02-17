// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisAnalyzerTests.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Analyzer.Tests
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Mocklis.Analyzer.Tests.Helpers;
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

        // ReSharper disable once AssignNullToNotNullAttribute
        private static string TestCaseFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestCases");

        public static TheoryData<string> EnumerateTestCases()
        {
            var data = new TheoryData<string>();

            foreach (var file in Directory.EnumerateFiles(TestCaseFolder, "*.cs"))
            {
                if (!file.EndsWith(".Expected.cs"))
                {
                    data.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

            return data;
        }

        [Theory]
        [MemberData(nameof(EnumerateTestCases))]
        public async Task TestMocklisClassUpdater(string testCase)
        {
            string source = File.ReadAllText(Path.Combine(TestCaseFolder, testCase + ".cs"));
            string expectedFileName = Path.Combine(TestCaseFolder, testCase + ".Expected.cs");
            string expected = null;
            if (File.Exists(expectedFileName))
            {
                expected = File.ReadAllText(expectedFileName);
            }

            var result = await _mocklisClassUpdater.UpdateMocklisClass(source);

            // Uncomment to create expected value for regression purposes
#if !NCRUNCH
            if (expected == null || expected != result.Code)
            {
                string newPath = Path.Combine(TestCaseFolder, "..", "..", "..", "..", "TestCases", testCase + ".Expected.cs");
                File.WriteAllText(newPath, result.Code);
                expected = result.Code;
            }
#endif

            if (result.IsSuccess)
            {
                Assert.Equal(expected, result.Code);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _testOutputHelper.WriteLine(error);
                }

                throw new Exception("Compilation failed...");
            }
        }
    }
}
