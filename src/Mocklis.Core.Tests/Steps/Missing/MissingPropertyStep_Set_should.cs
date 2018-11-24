// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingPropertyStep_Set_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Steps.Missing
{
    #region Using Directives

    using Mocklis.Steps.Missing;
    using Xunit;

    #endregion

    public class MissingPropertyStep_Set_should
    {
        private readonly PropertyMock<int> _propertyMock;
        private readonly MissingPropertyStep<int> _missingPropertyStep;

        public MissingPropertyStep_Set_should()
        {
            _propertyMock = new PropertyMock<int>(new object(), "TestClass", "ITest", "Indexer", "Indexer_1");
            _missingPropertyStep = MissingPropertyStep<int>.Instance;
        }

        [Fact]
        public void throw_exception()
        {
            var exception = Assert.Throws<MockMissingException>(() => _missingPropertyStep.Set(_propertyMock, 0));
            Assert.Equal(MockType.PropertySet, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Indexer", exception.MemberName);
            Assert.Equal("Indexer_1", exception.MemberMockName);
        }
    }
}
