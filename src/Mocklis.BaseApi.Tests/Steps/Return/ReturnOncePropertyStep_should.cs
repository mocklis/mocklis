// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOncePropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ReturnOncePropertyStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnceAndForwardThereafter()
        {
            MockMembers.IntProperty.ReturnOnce(45);
            int result1 = Sut.IntProperty;
            int result2 = Sut.IntProperty;

            Assert.Equal(45, result1);
            Assert.Equal(0, result2);
        }
    }
}
