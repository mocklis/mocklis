// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOnceMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ReturnOnceMethodStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnceAndForwardThereafter()
        {
            MockMembers.FuncWithParameter.ReturnOnce(45);
            var result1 = Sut.FuncWithParameter(14);
            var result2 = Sut.FuncWithParameter(99);

            Assert.Equal(45, result1);
            Assert.Equal(0, result2);
        }
    }
}
