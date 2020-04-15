// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesIndexerStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class TimesIndexerStep_should
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IIndexers Sut => MockMembers;

        [Fact]
        public void UseSameCounterForGetsAndSets()
        {
            MockMembers.Item
                .Times(5, step => step.StoredAsDictionary())
                .StoredAsDictionary();

            // First 5 invocations will go to the first dictionary
            Sut[1] = "One";
            Sut[2] = "Two";
            Sut[3] = "Three";
            var result1 = Sut[1];
            var result2 = Sut[2];

            // And the remaining 3 to the second...
            var result3 = Sut[3];
            Sut[4] = "Four";
            var result4 = Sut[4];

            Assert.Equal("One", result1);
            Assert.Equal("Two", result2);
            Assert.Null(result3);
            Assert.Equal("Four", result4);
        }
    }
}
