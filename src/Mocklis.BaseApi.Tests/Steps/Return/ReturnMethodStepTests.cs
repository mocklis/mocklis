// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ReturnMethodStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnEveryCall()
        {
            MockMembers.FuncWithParameter.Return(45);
            int result1 = Sut.FuncWithParameter(14);
            int result2 = Sut.FuncWithParameter(99);

            Assert.Equal(45, result1);
            Assert.Equal(45, result2);
        }
    }
}
