// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ReturnEachIndexerStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void ReturnGivenValuesInTurnAndForwardThereafter()
        {
            MockMembers.Item.ReturnEach("Test", "Test2");
            string result1 = Sut[14];
            string result2 = Sut[99];
            string result3 = Sut[23];
            string result4 = Sut[73];

            Assert.Equal("Test", result1);
            Assert.Equal("Test2", result2);
            Assert.Null(result3);
            Assert.Null(result4);
        }

        [Fact]
        public void NoGivenValuesForwardStraightAway()
        {
            MockMembers.Item.ReturnEach();
            string result1 = Sut[14];

            Assert.Null(result1);
        }
    }
}
