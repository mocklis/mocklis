// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyPropertyStep_Get_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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
            _mockMembers.StringProperty.Dummy();
            _mockMembers.IntProperty.Dummy();
            Assert.Null(((IProperties)_mockMembers).StringProperty);
            Assert.Equal(0, ((IProperties)_mockMembers).IntProperty);
        }
    }
}
