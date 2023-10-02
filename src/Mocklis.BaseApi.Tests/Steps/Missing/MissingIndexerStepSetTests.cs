// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingIndexerStepSetTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;
    using Xunit;

    #endregion

    public class MissingIndexerStepSetTests
    {
        private readonly IndexerMock<int, string> _indexerMock;
        private readonly MissingIndexerStep<int, string> _missingIndexerStep;

        public MissingIndexerStepSetTests()
        {
            _indexerMock = new IndexerMock<int, string>(new object(), "TestClass", "ITest", "Indexer", "Indexer1", Strictness.Lenient);
            _missingIndexerStep = MissingIndexerStep<int, string>.Instance;
        }

        [Fact]
        public void ThrowException()
        {
            var exception = Assert.Throws<MockMissingException>(() => _missingIndexerStep.Set(_indexerMock, 0, "newValue"));
            Assert.Equal(MockType.IndexerSet, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Indexer", exception.MemberName);
            Assert.Equal("Indexer1", exception.MemberMockName);
        }
    }
}
