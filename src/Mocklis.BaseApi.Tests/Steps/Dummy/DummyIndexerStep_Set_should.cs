// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyIndexerStep_Set_should.cs">
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

    public class DummyIndexerStep_Set_should
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void not_throw()
        {
            _mockMembers.Item.Dummy();
            ((IIndexers)_mockMembers)[5] = "test";
        }
    }
}