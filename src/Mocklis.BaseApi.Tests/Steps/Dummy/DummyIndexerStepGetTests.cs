// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyIndexerStepGetTests.cs">
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

    public class DummyIndexerStepGetTests
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void ReturnDefaultValue()
        {
            _mockMembers.Item.Dummy();
            Assert.Null(((IIndexers)_mockMembers)[5]);
        }
    }
}
