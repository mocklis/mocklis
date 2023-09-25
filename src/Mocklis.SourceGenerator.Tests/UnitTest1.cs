using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Mocklis.SourceGenerator;

using System.IO;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

public sealed class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test1()
    {
        var compilation = CSharpCompilation.Create("TestProject",
            new[] { CSharpSyntaxTree.ParseText("using System;namespace MyTestNs;[Mocklis.Core.MocklisClassAttribute]public partial class Test : IDisposable { }") },
            TestReferences.MetadataReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,nullableContextOptions: NullableContextOptions.Enable));

        var generator = new MocklisSourceGenerator();
        var sourceGenerator = generator.AsSourceGenerator();

        // trackIncrementalGeneratorSteps allows to report info about each step of the generator
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators: new ISourceGenerator[] { sourceGenerator },
            driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));

        // Run the generator
        driver = driver.RunGenerators(compilation);

        var results = driver.GetRunResult().Results.Single();

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
        var sb = new StringBuilder();
        TextWriter sw = new StringWriter(sb);
        results.GeneratedSources.Single().SourceText.Write(sw);

        var x = sb.ToString();
        _testOutputHelper.WriteLine(x);


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
}
