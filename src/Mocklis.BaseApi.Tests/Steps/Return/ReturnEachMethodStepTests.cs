// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachMethodStepTests.cs">
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

    public class ReturnEachMethodStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnceAndForwardThereafter()
        {
            MockMembers.FuncWithParameter.ReturnEach(45, 54);
            var result1 = Sut.FuncWithParameter(14);
            var result2 = Sut.FuncWithParameter(99);
            var result3 = Sut.FuncWithParameter(23);
            var result4 = Sut.FuncWithParameter(73);

            Assert.Equal(45, result1);
            Assert.Equal(54, result2);
            Assert.Equal(0, result3);
            Assert.Equal(0, result4);
        }
    }
}
