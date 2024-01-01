// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOncePropertyStepTests.cs">
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

    public class ReturnOncePropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnceAndForwardThereafter()
        {
            MockMembers.IntProperty.ReturnOnce(45);
            var result1 = Sut.IntProperty;
            var result2 = Sut.IntProperty;

            Assert.Equal(45, result1);
            Assert.Equal(0, result2);
        }
    }
}
