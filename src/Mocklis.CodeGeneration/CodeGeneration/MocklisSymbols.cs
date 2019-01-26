// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisSymbols.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

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
        public INamedTypeSymbol RuntimeArgumentHandle { get; }

        private INamedTypeSymbol Object { get; }
        private Compilation Compilation { get; }

        public MocklisSymbols(Compilation compilation)
        {
            Compilation = compilation;
            MocklisClassAttribute = compilation.GetTypeByMetadataName("Mocklis.Core.MocklisClassAttribute");
            ActionMethodMock0 = compilation.GetTypeByMetadataName("Mocklis.Core.ActionMethodMock");
            ActionMethodMock1 = compilation.GetTypeByMetadataName("Mocklis.Core.ActionMethodMock`1");
            EventMock1 = compilation.GetTypeByMetadataName("Mocklis.Core.EventMock`1");
            FuncMethodMock1 = compilation.GetTypeByMetadataName("Mocklis.Core.FuncMethodMock`1");
            FuncMethodMock2 = compilation.GetTypeByMetadataName("Mocklis.Core.FuncMethodMock`2");
            IndexerMock2 = compilation.GetTypeByMetadataName("Mocklis.Core.IndexerMock`2");
            PropertyMock1 = compilation.GetTypeByMetadataName("Mocklis.Core.PropertyMock`1");
            MockMissingException = compilation.GetTypeByMetadataName("Mocklis.Core.MockMissingException");
            MockType = compilation.GetTypeByMetadataName("Mocklis.Core.MockType");
            ByRef1 = compilation.GetTypeByMetadataName("Mocklis.Core.ByRef`1");
            TypedMockProvider = compilation.GetTypeByMetadataName("Mocklis.Core.TypedMockProvider");
            RuntimeArgumentHandle = compilation.GetTypeByMetadataName("System.RuntimeArgumentHandle");
            Object = compilation.GetTypeByMetadataName("System.Object");
        }

        public bool HasImplicitConversionToObject(ITypeSymbol symbol)
        {
            return Compilation.HasImplicitConversion(symbol, Object);
        }
    }
}
