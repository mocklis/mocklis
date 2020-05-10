// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyMethodStepCallTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class DummyMethodStepCallTests
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void ReturnDefaultValue()
        {
            _mockMembers.FuncWithParameter.Dummy();
            var result = ((IMethods)_mockMembers).FuncWithParameter(150);
            Assert.Equal(default, result);
        }
    }
}
