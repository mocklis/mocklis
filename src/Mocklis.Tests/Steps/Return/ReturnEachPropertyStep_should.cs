// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachPropertyStep_should.cs">
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

    public class ReturnEachPropertyStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnceAndForwardThereafter()
        {
            MockMembers.IntProperty.ReturnEach(45, 54);
            int result1 = Sut.IntProperty;
            int result2 = Sut.IntProperty;
            int result3 = Sut.IntProperty;
            int result4 = Sut.IntProperty;

            Assert.Equal(45, result1);
            Assert.Equal(54, result2);
            Assert.Equal(0, result3);
            Assert.Equal(0, result4);
        }
    }
}
