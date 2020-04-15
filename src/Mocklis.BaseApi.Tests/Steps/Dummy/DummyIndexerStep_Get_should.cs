// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyIndexerStep_Get_should.cs">
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

    public class DummyIndexerStep_Get_should
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void return_default_value()
        {
            _mockMembers.Item.Dummy();
            Assert.Null(((IIndexers)_mockMembers)[5]);
        }
    }
}
