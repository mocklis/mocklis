// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class FuncMethodStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.SimpleFunc.Func((Func<int>)null!));
            Assert.Throws<ArgumentNullException>(() => MockMembers.FuncWithParameter.Func(null!));
        }

        [Fact]
        public void CallFuncWithNoParameters()
        {
            MockMembers.SimpleFunc.Func(() => 42);

            var result = Sut.SimpleFunc();

            Assert.Equal(42, result);
        }

        [Fact]
        public void CallFuncWithParameters()
        {
            var callParameter = 0;
            MockMembers.FuncWithParameter.Func(i =>
            {
                callParameter = i;
                return 42;
            });

            var result = Sut.FuncWithParameter(99);

            Assert.Equal(99, callParameter);
            Assert.Equal(42, result);
        }
    }
}
