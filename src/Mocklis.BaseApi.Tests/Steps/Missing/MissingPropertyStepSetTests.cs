// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingPropertyStepSetTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;
    using Xunit;

    #endregion

    public class MissingPropertyStepSetTests
    {
        private readonly PropertyMock<int> _propertyMock;
        private readonly MissingPropertyStep<int> _missingPropertyStep;

        public MissingPropertyStepSetTests()
        {
            _propertyMock = new PropertyMock<int>(new object(), "TestClass", "ITest", "Indexer", "Indexer1", Strictness.Lenient);
            _missingPropertyStep = MissingPropertyStep<int>.Instance;
        }

        [Fact]
        public void ThrowException()
        {
            var exception = Assert.Throws<MockMissingException>(() => _missingPropertyStep.Set(_propertyMock, 0));
            Assert.Equal(MockType.PropertySet, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Indexer", exception.MemberName);
            Assert.Equal("Indexer1", exception.MemberMockName);
        }
    }
}
