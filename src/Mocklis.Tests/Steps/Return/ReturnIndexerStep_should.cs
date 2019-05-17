// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnIndexerStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Return
{
    #region Using Directives

    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class ReturnIndexerStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnEveryCall()
        {
            MockMembers.Item.Return("Test");
            string result1 = Sut[14];
            string result2 = Sut[99];

            Assert.Equal("Test", result1);
            Assert.Equal("Test", result2);
        }
    }
}
