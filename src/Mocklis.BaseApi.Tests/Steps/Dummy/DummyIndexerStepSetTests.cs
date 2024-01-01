// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyIndexerStepSetTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class DummyIndexerStepSetTests
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void NotThrow()
        {
            _mockMembers.Item.Dummy();
            ((IIndexers)_mockMembers)[5] = "test";
        }
    }
}
