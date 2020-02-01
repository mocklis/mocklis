// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingMethodStep_Call_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;
    using Mocklis.Steps.Missing;
    using Xunit;

    #endregion

    public class MissingMethodStep_Call_should
    {
        private readonly FuncMethodMock<int, string> _methodMock;
        private readonly MissingMethodStep<int, string> _missingMethodStep;

        public MissingMethodStep_Call_should()
        {
            _methodMock = new FuncMethodMock<int, string>(new object(), "TestClass", "ITest", "Method", "Method_1", Strictness.Lenient);
            _missingMethodStep = MissingMethodStep<int, string>.Instance;
        }

        [Fact]
        public void throw_exception()
        {
            var exception = Assert.Throws<MockMissingException>(() => _missingMethodStep.Call(_methodMock, 0));
            Assert.Equal(MockType.Method, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Method", exception.MemberName);
            Assert.Equal("Method_1", exception.MemberMockName);
        }
    }
}
