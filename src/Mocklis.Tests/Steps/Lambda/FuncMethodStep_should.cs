// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class FuncMethodStep_should
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
            int callParameter = 0;
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
