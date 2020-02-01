// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Times
{
    #region Using Directives

    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class TimesPropertyStep_should
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IProperties Sut => MockMembers;

        [Fact]
        public void UseSameCounterForGetsAndSets()
        {
            MockMembers.IntProperty
                .Times(3, step => step.Stored())
                .Stored(99);

            var result1 = Sut.IntProperty;
            Sut.IntProperty = 16;
            var result2 = Sut.IntProperty;
            var result3 = Sut.IntProperty;
            Sut.IntProperty = 8;
            var result4 = Sut.IntProperty;

            Assert.Equal(0, result1);
            Assert.Equal(16, result2);
            Assert.Equal(99, result3);
            Assert.Equal(8, result4);
        }
    }
}
