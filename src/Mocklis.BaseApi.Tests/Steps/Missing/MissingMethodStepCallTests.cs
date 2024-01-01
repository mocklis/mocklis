// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingMethodStepCallTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;
    using Xunit;

    #endregion

    public class MissingMethodStepCallTests
    {
        private readonly FuncMethodMock<int, string> _methodMock;
        private readonly MissingMethodStep<int, string> _missingMethodStep;

        public MissingMethodStepCallTests()
        {
            _methodMock = new FuncMethodMock<int, string>(new object(), "TestClass", "ITest", "Method", "Method1", Strictness.Lenient);
            _missingMethodStep = MissingMethodStep<int, string>.Instance;
        }

        [Fact]
        public void ThrowException()
        {
            var exception = Assert.Throws<MockMissingException>(() => _missingMethodStep.Call(_methodMock, 0));
            Assert.Equal(MockType.Method, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Method", exception.MemberName);
            Assert.Equal("Method1", exception.MemberMockName);
        }
    }
}
