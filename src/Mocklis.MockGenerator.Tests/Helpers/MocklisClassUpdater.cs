// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassUpdater.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.Helpers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Emit;
    using Microsoft.CodeAnalysis.Text;
    using Mocklis.SourceGenerator;

    #endregion

    public class MocklisClassUpdater
    {
        #region Static

        public static AdhocWorkspace CreateWorkspace(string source, LanguageVersion languageVersion, out ProjectId projectId, out DocumentId documentId)
        {
            string projectName = "TestProject";
            projectId = ProjectId.CreateNewId(debugName: projectName);

            var documentFileName = "Test.cs";

            var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, projectName, projectName, LanguageNames.CSharp,
                compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                parseOptions: new CSharpParseOptions(languageVersion),
                metadataReferences: TestReferences.MetadataReferences);

            var result = new AdhocWorkspace();
            result.AddProject(projectInfo);
            var document = result.AddDocument(projectId, documentFileName, SourceText.From(source));
            documentId = document.Id;
            return result;
        }

        private static Document CreateDocument(string source, LanguageVersion languageVersion)
        {
            var workspace = CreateWorkspace(source, languageVersion, out var projectId, out var documentId);
            return workspace.CurrentSolution.GetProject(projectId)!.GetDocument(documentId)!;
        }

        #endregion

        private readonly MocklisAnalyzer _mocklisAnalyzer = new();
        private readonly MocklisCodeFixProvider _mocklisCodeFixProvider = new();

        public async Task<MocklisClassUpdaterResult> UpdateMocklisClass(string source, LanguageVersion languageVersion)
        {
            var document = CreateDocument(source, languageVersion);

            var compilation = await document.Project.GetCompilationAsync();
            if (compilation == null)
            {
                throw new InvalidOperationException("Could not update Mocklis class since compilation can't be found.");
            }

            var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(_mocklisAnalyzer));
            var diagnostics = (await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync()).Single();

            CodeAction? action = null;
            var context = new CodeFixContext(document, diagnostics, (a, _) => action = a, CancellationToken.None);
            await _mocklisCodeFixProvider.RegisterCodeFixesAsync(context);

            // Action will have been created by the call to CodeFixContext
            var operations = await action!.GetOperationsAsync(CancellationToken.None);
            var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
            var updatedDocument = solution.GetDocument(document.Id)!;
            var root = await updatedDocument.GetSyntaxRootAsync();

            if (root == null)
            {
                throw new InvalidOperationException("Could not find updated root node.");
            }

            var code = root.GetText().ToString();

            var project = updatedDocument.Project;
            var newCompilation = await project.GetCompilationAsync() ?? throw new InvalidOperationException("Could not create compilation.");
            return BuildCompilation(newCompilation, code);
        }

        public static MocklisClassUpdaterResult BuildCompilation(Compilation newCompilation, string code)
        {
            using (var ms = new MemoryStream())
            {
                EmitResult emitResult = newCompilation.Emit(ms);
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
