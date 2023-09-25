// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SemanticModelExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.Compatibility
{
    #region Using Directives

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public static class SemanticModelExtensions
    {
        public static bool ClassIsInNullableContext(this SemanticModel semanticModel, ClassDeclarationSyntax classDecl)
        {
            return semanticModel.GetNullableContext(classDecl.Span.Start).AnnotationsEnabled();
        }
    }
}
