// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock_Add_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using Xunit;

    #endregion

    public class MocklisClassAttribute_constructor_should
    {
        [Fact]
        public void set_correct_property_values()
        {
            var sut = new MocklisClassAttribute();

            Assert.False(sut.MockReturnsByRef);
            Assert.True(sut.MockReturnsByRefReadonly);
        }
    }
}
