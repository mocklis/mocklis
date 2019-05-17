// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOnceMethodStep_should.cs">
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

    public class ReturnOnceMethodStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnceAndForwardThereafter()
        {
            MockMembers.FuncWithParameter.ReturnOnce(45);
            int result1 = Sut.FuncWithParameter(14);
            int result2 = Sut.FuncWithParameter(99);

            Assert.Equal(45, result1);
            Assert.Equal(0, result2);
        }
    }
}
