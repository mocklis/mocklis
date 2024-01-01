// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReferences.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.Helpers
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Mocklis.Core;

    #endregion

    public static class TestReferences
    {
        private static readonly MetadataReference CorlibReference;
        private static readonly MetadataReference SystemLinqReference;
        private static readonly MetadataReference SystemDiagnosticsReference;
        private static readonly MetadataReference MocklisCoreReference;
        private static readonly MetadataReference RuntimeReference;
        private static readonly MetadataReference NetStandardReference;

        static TestReferences()
        {
            CorlibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            SystemLinqReference = MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);
            SystemDiagnosticsReference = MetadataReference.CreateFromFile(typeof(GeneratedCodeAttribute).Assembly.Location);
            MocklisCoreReference = MetadataReference.CreateFromFile(typeof(MocklisClassAttribute).Assembly.Location);
            RuntimeReference = MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=0.0.0.0").Location);
            NetStandardReference = MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.1.0.0").Location);
        }

        public static IEnumerable<MetadataReference> MetadataReferences =>
            new[]
            {
                CorlibReference, SystemLinqReference, SystemDiagnosticsReference, MocklisCoreReference, RuntimeReference, NetStandardReference
            };
    }
}
