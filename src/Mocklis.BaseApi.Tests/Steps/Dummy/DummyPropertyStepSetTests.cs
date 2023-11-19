// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyPropertyStepSetTests.cs">
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

    public class DummyPropertyStepSetTests
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void NotThrow()
        {
            _mockMembers.StringProperty.Dummy();
            _mockMembers.IntProperty.Dummy();

            ((IProperties)_mockMembers).StringProperty = "test";
            ((IProperties)_mockMembers).IntProperty = 5;
        }
    }
}
