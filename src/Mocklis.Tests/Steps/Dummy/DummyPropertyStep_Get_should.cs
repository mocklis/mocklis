// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyPropertyStep_Get_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class DummyPropertyStep_Get_should
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void return_default_value()
        {
            _mockMembers.Name.Dummy();
            _mockMembers.Age.Dummy();
            Assert.Null(((IProperties)_mockMembers).Name);
            Assert.Equal(0, ((IProperties)_mockMembers).Age);
        }
    }
}
