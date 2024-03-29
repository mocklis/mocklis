// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnPropertyStepTests.cs">
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

    public class ReturnPropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void ReturnGivenValueOnEveryCall()
        {
            MockMembers.IntProperty.Return(45);
            var result1 = Sut.IntProperty;
            var result2 = Sut.IntProperty;

            Assert.Equal(45, result1);
            Assert.Equal(45, result2);
        }
    }
}
