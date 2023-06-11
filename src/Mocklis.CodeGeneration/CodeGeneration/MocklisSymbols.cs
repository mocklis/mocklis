// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisSymbols.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using Microsoft.CodeAnalysis;

    #endregion

    public class MocklisSymbols
    {
        public INamedTypeSymbol MocklisClassAttribute { get; }
        public INamedTypeSymbol ActionMethodMock0 { get; }
        public INamedTypeSymbol ActionMethodMock1 { get; }
        public INamedTypeSymbol EventMock1 { get; }
        public INamedTypeSymbol FuncMethodMock1 { get; }
        public INamedTypeSymbol FuncMethodMock2 { get; }
        public INamedTypeSymbol IndexerMock2 { get; }
        public INamedTypeSymbol PropertyMock1 { get; }
        public INamedTypeSymbol MockMissingException { get; }
        public INamedTypeSymbol MockType { get; }
        public INamedTypeSymbol ByRef1 { get; }
        public INamedTypeSymbol TypedMockProvider { get; }
        public INamedTypeSymbol Strictness { get; }
        public INamedTypeSymbol RuntimeArgumentHandle { get; }
        public INamedTypeSymbol GeneratedCodeAttribute { get; }

        private INamedTypeSymbol Object { get; }
        private Compilation Compilation { get; }

        public MocklisSymbols(Compilation compilation)
        {
            INamedTypeSymbol GetTypeSymbol(string metadataName)
            {
                return compilation.GetTypeByMetadataName(metadataName) ??
                       throw new ArgumentException($"Compilation does not contain {metadataName}.", nameof(compilation));
            }

            Compilation = compilation;
            MocklisClassAttribute = GetTypeSymbol("Mocklis.Core.MocklisClassAttribute");
            ActionMethodMock0 = GetTypeSymbol("Mocklis.Core.ActionMethodMock");
            ActionMethodMock1 = GetTypeSymbol("Mocklis.Core.ActionMethodMock`1");
            EventMock1 = GetTypeSymbol("Mocklis.Core.EventMock`1");
            FuncMethodMock1 = GetTypeSymbol("Mocklis.Core.FuncMethodMock`1");
            FuncMethodMock2 = GetTypeSymbol("Mocklis.Core.FuncMethodMock`2");
            IndexerMock2 = GetTypeSymbol("Mocklis.Core.IndexerMock`2");
            PropertyMock1 = GetTypeSymbol("Mocklis.Core.PropertyMock`1");
            MockMissingException = GetTypeSymbol("Mocklis.Core.MockMissingException");
            MockType = GetTypeSymbol("Mocklis.Core.MockType");
            ByRef1 = GetTypeSymbol("Mocklis.Core.ByRef`1");
            TypedMockProvider = GetTypeSymbol("Mocklis.Core.TypedMockProvider");
            Strictness = GetTypeSymbol("Mocklis.Core.Strictness");
            RuntimeArgumentHandle = GetTypeSymbol("System.RuntimeArgumentHandle");
            GeneratedCodeAttribute = GetTypeSymbol("System.CodeDom.Compiler.GeneratedCodeAttribute");
            Object = GetTypeSymbol("System.Object");
        }

        public bool HasImplicitConversionToObject(ITypeSymbol symbol)
        {
            return Compilation.HasImplicitConversion(symbol, Object);
        }
    }
}
