// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class TimesPropertyStepTests
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
