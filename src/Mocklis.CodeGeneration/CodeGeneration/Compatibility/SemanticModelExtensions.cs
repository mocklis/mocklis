// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SemanticModelExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.Compatibility
{
    #region Using Directives

    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public static class SemanticModelExtensions
    {
        private static readonly MethodInfo GetNullableContextMethodInfo = typeof(SemanticModel).GetMethod("GetNullableContext");

        public static bool ClassIsInNullableContext(this SemanticModel semanticModel, ClassDeclarationSyntax classDecl)
        {
            if (GetNullableContextMethodInfo == null)
            {
                return false;
            }

            var result = (int)GetNullableContextMethodInfo.Invoke(semanticModel, new object[] { classDecl.Span.Start });
            return (result & 2) != 0;
        }
    }
}
