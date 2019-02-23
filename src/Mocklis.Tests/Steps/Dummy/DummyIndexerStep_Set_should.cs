// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyIndexerStep_Set_should.cs">
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
