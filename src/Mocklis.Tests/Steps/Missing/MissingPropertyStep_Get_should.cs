// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingPropertyStep_Get_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;
    using Mocklis.Steps.Missing;
    using Xunit;

    #endregion

    public class MissingPropertyStep_Get_should
    {
        private readonly PropertyMock<int> _propertyMock;
        private readonly MissingPropertyStep<int> _missingPropertyStep;

        public MissingPropertyStep_Get_should()
        {
            _propertyMock = new PropertyMock<int>(new object(), "TestClass", "ITest", "Indexer", "Indexer_1", Strictness.Lenient);
            _missingPropertyStep = MissingPropertyStep<int>.Instance;
        }

        [Fact]
        public void throw_exception()
        {
            var exception = Assert.Throws<MockMissingException>(() => _missingPropertyStep.Get(_propertyMock));
            Assert.Equal(MockType.PropertyGet, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Indexer", exception.MemberName);
            Assert.Equal("Indexer_1", exception.MemberMockName);
        }
    }
}
