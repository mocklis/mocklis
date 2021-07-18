// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassUpdater.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.Helpers
{
    #region Using Directives

    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
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
        private static readonly MetadataReference SystemLinqReference;
        private static readonly MetadataReference SystemDiagnosticsReference;
        private static readonly MetadataReference MocklisCoreReference;
        private static readonly MetadataReference RuntimeReference;
        private static readonly MetadataReference NetStandardReference;

        static MocklisClassUpdater()
        {
            CorlibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            SystemLinqReference = MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);
            SystemDiagnosticsReference = MetadataReference.CreateFromFile(typeof(GeneratedCodeAttribute).Assembly.Location);
            MocklisCoreReference = MetadataReference.CreateFromFile(typeof(MocklisClassAttribute).Assembly.Location);
            RuntimeReference = MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=0.0.0.0").Location);
            NetStandardReference = MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.1.0.0").Location);
        }

        private static Document CreateDocument(string source, LanguageVersion languageVersion)
        {
            string projectName = "TestProject";
            var projectId = ProjectId.CreateNewId(debugName: projectName);

            var documentFileName = "Test.cs";
            var documentId = DocumentId.CreateNewId(projectId, debugName: documentFileName);

            var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, projectName, projectName, LanguageNames.CSharp,
                compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                parseOptions: new CSharpParseOptions(languageVersion),
                metadataReferences: new[]
                {
                    CorlibReference, SystemLinqReference, SystemDiagnosticsReference, MocklisCoreReference, RuntimeReference, NetStandardReference
                });

            var solution = new AdhocWorkspace()
                .CurrentSolution
                .AddProject(projectInfo)
                .AddDocument(documentId, documentFileName, SourceText.From(source));

            // We know that projects and documents exist since we've just added them.
            return solution.GetProject(projectId)!.GetDocument(documentId)!;
        }

        #endregion

        private readonly MocklisAnalyzer _mocklisAnalyzer;
        private readonly MocklisCodeFixProvider _mocklisCodeFixProvider;

        public MocklisClassUpdater()
        {
            _mocklisAnalyzer = new MocklisAnalyzer();
            _mocklisCodeFixProvider = new MocklisCodeFixProvider();
        }

        public async Task<MocklisClassUpdaterResult> UpdateMocklisClass(string source, LanguageVersion languageVersion)
        {
            var document = CreateDocument(source, languageVersion);

            var compilation = await document.Project.GetCompilationAsync();
            var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(_mocklisAnalyzer));
            var diagnostics = (await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync()).Single();

            CodeAction? action = null;
            var context = new CodeFixContext(document, diagnostics, (a, d) => action = a, CancellationToken.None);
            await _mocklisCodeFixProvider.RegisterCodeFixesAsync(context);

            // Action will have been created by the call to CodeFixContext
            var operations = await action!.GetOperationsAsync(CancellationToken.None);
            var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
            var updatedDocument = solution.GetDocument(document.Id)!;
            var root = await updatedDocument.GetSyntaxRootAsync();
            var code = root.GetText().ToString();

            var project = updatedDocument.Project;
            var newCompilation = await project.GetCompilationAsync();
            using (var ms = new MemoryStream())
            {
                EmitResult emitResult = newCompilation!.Emit(ms);
                if (!emitResult.Success)
                {
                    var codeLines = code.Split(Environment.NewLine);
                    var errors = emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error || d.IsWarningAsError);

                    var errorList = new List<MocklisClassUpdaterResult.Error>();
                    foreach (var error in errors)
                    {
                        string errorText = $"{error.Id}: {error.GetMessage()}";
                        var location = error.Location;
                        switch (location.Kind)
                        {
                            case LocationKind.SourceFile:
                            {
                                var span = location.GetLineSpan();
                                var lines = codeLines.Skip(span.StartLinePosition.Line)
                                    .Take(span.EndLinePosition.Line - span.StartLinePosition.Line + 1).ToArray();
                                errorList.Add(new MocklisClassUpdaterResult.Error(errorText, lines, span.StartLinePosition.Character,
                                    span.EndLinePosition.Character, span.StartLinePosition.Line + 1));
                                break;
                            }

                            default:
                            {
                                errorList.Add(new MocklisClassUpdaterResult.Error(errorText, Array.Empty<string>(), 0, 0, 0));
                                break;
                            }
                        }
                    }

                    return MocklisClassUpdaterResult.Failure(code, errorList.ToArray());
                }
            }

            return MocklisClassUpdaterResult.Success(code);
        }
    }
}
