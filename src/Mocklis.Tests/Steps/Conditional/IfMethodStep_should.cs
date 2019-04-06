// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class IfMethodStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;

        [Fact]
        public void check_condition()
        {
            MockMembers.FuncWithParameter
                .If(a => a < 5, s => s.Func(v => v * 2))
                .Func(v => v * 4);

            Assert.Equal(4, Sut.FuncWithParameter(2));
            Assert.Equal(24, Sut.FuncWithParameter(6));
        }

        [Fact]
        public void use_ElseBranch_if_asked()
        {
            var vg = new VerificationGroup();
            MockMembers.FuncWithParameter
                .If(_ => true, s => s.ExpectedUsage(vg, "IfBranch", 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1).Dummy();

            Sut.FuncWithParameter(5);

            vg.Assert();
        }

        [Fact]
        public void throw_when_passed_null_as_NextStep()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MockMembers.FuncWithParameter.If(
                    v => true,
                    s => ((ICanHaveNextMethodStep<int, int>)s).SetNextStep((IMethodStep<int, int>)null)
                )
            );
        }
    }
}
