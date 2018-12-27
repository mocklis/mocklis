// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisSymbols.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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
        public INamedTypeSymbol Action { get; }
        public INamedTypeSymbol Action1 { get; }
        public INamedTypeSymbol ValueTuple { get; }
        public INamedTypeSymbol EventStepCaller1 { get; }
        public INamedTypeSymbol IndexerStepCaller2 { get; }
        public INamedTypeSymbol MethodStepCaller2 { get; }
        public INamedTypeSymbol PropertyStepCaller1 { get; }
        public INamedTypeSymbol MockMissingException { get; }
        public INamedTypeSymbol MockType { get; }
        public INamedTypeSymbol RuntimeArgumentHandle { get; }

        public MocklisSymbols(Compilation compilation)
        {
            MocklisClassAttribute = compilation.GetTypeByMetadataName("Mocklis.Core.MocklisClassAttribute");
            ActionMethodMock0 = compilation.GetTypeByMetadataName("Mocklis.Core.ActionMethodMock");
            ActionMethodMock1 = compilation.GetTypeByMetadataName("Mocklis.Core.ActionMethodMock`1");
            EventMock1 = compilation.GetTypeByMetadataName("Mocklis.Core.EventMock`1");
            FuncMethodMock1 = compilation.GetTypeByMetadataName("Mocklis.Core.FuncMethodMock`1");
            FuncMethodMock2 = compilation.GetTypeByMetadataName("Mocklis.Core.FuncMethodMock`2");
            IndexerMock2 = compilation.GetTypeByMetadataName("Mocklis.Core.IndexerMock`2");
            PropertyMock1 = compilation.GetTypeByMetadataName("Mocklis.Core.PropertyMock`1");
            Action = compilation.GetTypeByMetadataName("System.Action");
            Action1 = compilation.GetTypeByMetadataName("System.Action`1");
            ValueTuple = compilation.GetTypeByMetadataName("System.ValueTuple");
            EventStepCaller1 = compilation.GetTypeByMetadataName("Mocklis.Core.IEventStepCaller`1");
            IndexerStepCaller2 = compilation.GetTypeByMetadataName("Mocklis.Core.IIndexerStepCaller`2");
            MethodStepCaller2 = compilation.GetTypeByMetadataName("Mocklis.Core.IMethodStepCaller`2");
            PropertyStepCaller1 = compilation.GetTypeByMetadataName("Mocklis.Core.IPropertyStepCaller`1");
            MockMissingException = compilation.GetTypeByMetadataName("Mocklis.Core.MockMissingException");
            MockType = compilation.GetTypeByMetadataName("Mocklis.Core.MockType");
            RuntimeArgumentHandle = compilation.GetTypeByMetadataName("System.RuntimeArgumentHandle");
        }
    }
}
