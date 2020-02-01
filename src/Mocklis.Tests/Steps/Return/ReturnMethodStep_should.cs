// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMethodStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Return
{
    #region Using Directives

    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class ReturnMethodStep_should
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
