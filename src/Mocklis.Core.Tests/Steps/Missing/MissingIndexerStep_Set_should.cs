// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingIndexerStep_Set_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Steps.Missing
{
    #region Using Directives

    using Mocklis.Steps.Missing;
    using Xunit;

    #endregion

    public class MissingIndexerStep_Set_should
    {
        private readonly IndexerMock<int, string> _indexerMock;
        private readonly MissingIndexerStep<int, string> _missingIndexerStep;

        public MissingIndexerStep_Set_should()
        {
            _indexerMock = new IndexerMock<int, string>(new object(), "TestClass", "ITest", "Indexer", "Indexer_1");
            _missingIndexerStep = MissingIndexerStep<int, string>.Instance;
        }

        [Fact]
        public void throw_exception()
        {
            var exception = Assert.Throws<MockMissingException>(() => _missingIndexerStep.Set(_indexerMock, 0, "newValue"));
            Assert.Equal(MockType.IndexerSet, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Indexer", exception.MemberName);
            Assert.Equal("Indexer_1", exception.MemberMockName);
        }
    }
}
