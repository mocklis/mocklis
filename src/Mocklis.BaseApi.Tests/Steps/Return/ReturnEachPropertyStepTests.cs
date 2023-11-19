// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachPropertyStepTests.cs">
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

    public class ReturnEachPropertyStepTests
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
