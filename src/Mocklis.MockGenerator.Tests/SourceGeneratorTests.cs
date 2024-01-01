// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceGeneratorTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator;

#region Using Directives

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mocklis.MockGenerator.Helpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

#endregion

public sealed class SourceGeneratorTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SourceGeneratorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
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

        Task<string> expectedSourceTask = Task.FromResult(string.Empty);
        if (File.Exists(test.ExpectedSourceFileName))
        {
            expectedSourceTask = File.ReadAllTextAsync(test.ExpectedSourceFileName);
        }

        string source = await sourceTask.ConfigureAwait(false);

        source = source.Replace("[PARTIAL] ", "partial ");

        var workSpace = MocklisClassUpdater.CreateWorkspace(source, languageVersion, out var projectId, out _);

        var project = workSpace.CurrentSolution.GetProject(projectId)!;

        var compilation = await project.GetCompilationAsync() ?? throw new InvalidOperationException("Could not create compilation.");

        var generator = new MocklisSourceGenerator();
        var sourceGenerator = generator.AsSourceGenerator();

        // trackIncrementalGeneratorSteps allows to report info about each step of the generator
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators: new[] { sourceGenerator },
            driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));

        // Run the generator
        driver = driver.RunGenerators(compilation);

        var results = driver.GetRunResult().Results.Single();

        foreach (var generatedSource in results.GeneratedSources)
        {
            workSpace.AddDocument(projectId, generatedSource.HintName, generatedSource.SourceText);
        }

        var newProject = workSpace.CurrentSolution.GetProject(projectId)!;

        var newCompilation = await newProject.GetCompilationAsync() ?? throw new InvalidOperationException("Could not create compilation.");

        var result = MocklisClassUpdater.BuildCompilation(newCompilation, "");

        var sb = new StringBuilder();
        TextWriter sw = new StringWriter(sb);
        results.GeneratedSources.Single().SourceText.Write(sw);

        var resultingCode = sb.ToString();

        _testOutputHelper.WriteLine(resultingCode);

        string expected = await expectedSourceTask.ConfigureAwait(false);

        // Create the 'expected' file if it isn't there. Comment out next line and run tests manually to recreate.
        if (string.IsNullOrWhiteSpace(expected))
        {
#if NCRUNCH
            var folder = Environment.GetEnvironmentVariable("MockGeneratorTestsFolder");
            string expectedFilePathInSourceCode =
                folder == null ? string.Empty : Path.Combine(folder, test.TestCaseFolder, test.TestCase + ".ExpectedSource.cs");
#else
            string expectedFilePathInSourceCode =
                Path.Combine(test.PathToTestCases, "..", "..", "..", test.TestCaseFolder, test.TestCase + ".ExpectedSource.cs");
#endif
            if (!string.IsNullOrWhiteSpace(expectedFilePathInSourceCode))
            {
                await File.WriteAllTextAsync(expectedFilePathInSourceCode, resultingCode).ConfigureAwait(false);
            }

            expected = resultingCode;
        }

        int i = 0;

        try
        {
            var e = expected.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var c = resultingCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

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

    //_testOutputHelper.WriteLine("TrackedSteps --------------------");
    //foreach (var item in results.TrackedSteps)
    //{
    //    _testOutputHelper.WriteLine($"{item.Key} -> {string.Join(',', item.Value.Select(a => $"{a.Name}:{a.ElapsedTime}"))}");
    //}
    //_testOutputHelper.WriteLine("TrackedOutputSteps ---------------");
    //foreach (var item in results.TrackedOutputSteps)
    //{
    //    _testOutputHelper.WriteLine($"{item.Key} -> {string.Join(',', item.Value.Select(a => $"{a.Name}:{a.ElapsedTime}"))}");
    //}
    //_testOutputHelper.WriteLine("Output ---------------------------");
    //var sb = new StringBuilder();
    //TextWriter sw = new StringWriter(sb);
    //results.GeneratedSources.Single().SourceText.Write(sw);

    //var x = sb.ToString();
    //_testOutputHelper.WriteLine(x);


    //// Update the compilation and rerun the generator
    //compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText("// dummy"));
    //driver = driver.RunGenerators(compilation);

    //// Assert the driver doesn't recompute the output
    //var result = driver.GetRunResult().Results.Single();
    //var allOutputs = result.TrackedOutputSteps.SelectMany(outputStep => outputStep.Value).SelectMany(output => output.Outputs);
    //Assert.Collection(allOutputs, output => Assert.Equal(IncrementalStepRunReason.Cached, output.Reason));

    //// Assert the driver use the cached result from AssemblyName and Syntax
    //var assemblyNameOutputs = result.TrackedSteps["AssemblyName"].Single().Outputs;
    //Assert.Collection(assemblyNameOutputs, output => Assert.Equal(IncrementalStepRunReason.Unchanged, output.Reason));

    //var syntaxOutputs = result.TrackedSteps["Syntax"].Single().Outputs;
    //Assert.Collection(syntaxOutputs, output => Assert.Equal(IncrementalStepRunReason.Unchanged, output.Reason));
}
