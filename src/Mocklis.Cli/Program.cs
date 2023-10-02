// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Cli
{
    #region Using Directives

    using System;
    using System.Threading.Tasks;
    using Microsoft.Build.Locator;
    using Microsoft.CodeAnalysis.MSBuild;

    #endregion

    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            MSBuildLocator.RegisterDefaults();

            using (var workspace = MSBuildWorkspace.Create())
            {
                var solution = await workspace.OpenSolutionAsync(args[0]);

                foreach (var projectId in solution.ProjectIds)
                {
                    var project = solution.GetProject(projectId);

                    if (project != null)
                    {
                        project = await ProjectInspector.GenerateMocklisClassContents(project);
                        solution = project.Solution;
                    }
                }

                if (!workspace.TryApplyChanges(solution))
                {
                    Console.WriteLine("Failed to apply changes...");
                }
            }

            return 0;
        }
    }
}
