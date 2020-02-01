// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyPropertyStep_Set_should.cs">
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

    public class DummyPropertyStep_Set_should
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void not_throw()
        {
            _mockMembers.StringProperty.Dummy();
            _mockMembers.IntProperty.Dummy();

            ((IProperties)_mockMembers).StringProperty = "test";
            ((IProperties)_mockMembers).IntProperty = 5;
        }
    }
}
