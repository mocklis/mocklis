// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingIndexerStep_Get_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Steps.Missing
{
    #region Using Directives

    using Mocklis.Steps.Missing;
    using Xunit;

    #endregion

    public class MissingIndexerStep_Get_should
    {
        private readonly IndexerMock<int, string> _indexerMock;
        private readonly MissingIndexerStep<int, string> _missingIndexerStep;

        public MissingIndexerStep_Get_should()
        {
            _indexerMock = new IndexerMock<int, string>(new object(), "TestClass", "ITest", "Indexer", "Indexer_1", Strictness.Lenient);
            _missingIndexerStep = MissingIndexerStep<int, string>.Instance;
        }

        [Fact]
        public void throw_exception()
        {
            var exception = Assert.Throws<MockMissingException>(() => _missingIndexerStep.Get(_indexerMock, 0));
            Assert.Equal(MockType.IndexerGet, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Indexer", exception.MemberName);
            Assert.Equal("Indexer_1", exception.MemberMockName);
        }
    }
}
