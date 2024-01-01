// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassAttributeConstructorTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using Xunit;

    #endregion

    public class MocklisClassAttributeConstructorTests
    {
        [Fact]
        public void SetCorrectPropertyValues()
        {
            var sut = new MocklisClassAttribute();

            Assert.False(sut.MockReturnsByRef);
            Assert.True(sut.MockReturnsByRefReadonly);
            Assert.False(sut.Strict);
            Assert.False(sut.VeryStrict);
        }
    }
}
