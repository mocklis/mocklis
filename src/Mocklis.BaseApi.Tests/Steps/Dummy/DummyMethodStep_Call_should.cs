// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyMethodStep_Call_should.cs">
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

    public class DummyMethodStep_Call_should
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void return_default_value()
        {
            _mockMembers.FuncWithParameter.Dummy();
            var result = ((IMethods)_mockMembers).FuncWithParameter(150);
            Assert.Equal(default, result);
        }
    }
}
