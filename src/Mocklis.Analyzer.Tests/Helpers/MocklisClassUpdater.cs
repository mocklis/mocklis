// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassUpdater.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Analyzer.Tests.Helpers
{
    #region Using Directives

    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Emit;
    using Microsoft.CodeAnalysis.Text;
    using Mocklis.Core;

    #endregion

    public class MocklisClassUpdater
    {
        #region Static

        private static readonly MetadataReference CorlibReference;
        private static readonly MetadataReference SystemCoreReference;
        private static readonly MetadataReference MocklisCoreReference;
        private static readonly MetadataReference RuntimeReference;
        private static readonly MetadataReference NetStandardReference;

        static MocklisClassUpdater()
        {
            CorlibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            SystemCoreReference = MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);
            MocklisCoreReference = MetadataReference.CreateFromFile(typeof(MocklisClassAttribute).Assembly.Location);
            RuntimeReference = MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=0.0.0.0").Location);
            NetStandardReference = MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0").Location);
        }

        private static Document CreateDocument(string source)
        {
            string projectName = "TestProject";
            var projectId = ProjectId.CreateNewId(debugName: projectName);

            var documentFileName = "Test.cs";
            var documentId = DocumentId.CreateNewId(projectId, debugName: documentFileName);

            var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, projectName, projectName, LanguageNames.CSharp,
                compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                parseOptions: new CSharpParseOptions(LanguageVersion.CSharp7_3),
                metadataReferences: new[] { CorlibReference, SystemCoreReference, MocklisCoreReference, RuntimeReference, NetStandardReference });

            var solution = new AdhocWorkspace()
                .CurrentSolution
                .AddProject(projectInfo)
                .AddDocument(documentId, documentFileName, SourceText.From(source));

            return solution.GetProject(projectId).GetDocument(documentId);
        }

        #endregion

        private readonly MocklisAnalyzer _mocklisAnalyzer;
        private readonly MocklisCodeFixProvider _mocklisCodeFixProvider;

        public MocklisClassUpdater()
        {
            _mocklisAnalyzer = new MocklisAnalyzer();
            _mocklisCodeFixProvider = new MocklisCodeFixProvider();
        }

        public async Task<MocklisClassUpdaterResult> UpdateMocklisClass(string source)
        {
            var document = CreateDocument(source);

            var compilation = await document.Project.GetCompilationAsync();
            var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(_mocklisAnalyzer));
            var diagnostics = (await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync()).Single();

            CodeAction action = null;
            var context = new CodeFixContext(document, diagnostics, (a, d) => action = a, CancellationToken.None);
            await _mocklisCodeFixProvider.RegisterCodeFixesAsync(context);

            var operations = await action.GetOperationsAsync(CancellationToken.None);
            var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
            var updatedDocument = solution.GetDocument(document.Id);

            var project = updatedDocument.Project;
            var newCompilation = await project.GetCompilationAsync();
            using (var ms = new MemoryStream())
            {
                EmitResult emitResult = newCompilation.Emit(ms);
                if (!emitResult.Success)
                {
                    var errors = emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error || d.IsWarningAsError)
                        .Select(d => d.Id + ": " + d.GetMessage()).ToArray();
                    return MocklisClassUpdaterResult.Failure(errors);
                }
            }

            var root = await updatedDocument.GetSyntaxRootAsync();
            return MocklisClassUpdaterResult.Success(root.GetText().ToString());
        }
    }
}
