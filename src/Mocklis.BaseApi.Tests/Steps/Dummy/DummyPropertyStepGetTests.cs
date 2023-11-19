// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyPropertyStepGetTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class DummyPropertyStepGetTests
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void ReturnDefaultValue()
        {
            _mockMembers.StringProperty.Dummy();
            _mockMembers.IntProperty.Dummy();
            Assert.Null(((IProperties)_mockMembers).StringProperty);
            Assert.Equal(0, ((IProperties)_mockMembers).IntProperty);
        }
    }
}
