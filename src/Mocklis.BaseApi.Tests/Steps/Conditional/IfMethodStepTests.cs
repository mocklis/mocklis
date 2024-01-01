// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class IfMethodStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;

        [Fact]
        public void RequireBranch()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.SimpleFunc.If(_ => true, null!));
        }

        [Fact]
        public void CheckCondition()
        {
            MockMembers.FuncWithParameter
                .If(a => a < 5, s => s.Func(v => v * 2))
                .Func(v => v * 4);

            Assert.Equal(4, Sut.FuncWithParameter(2));
            Assert.Equal(24, Sut.FuncWithParameter(6));
        }

        [Fact]
        public void UseElseBranchIfAsked()
        {
            var vg = new VerificationGroup();
            MockMembers.FuncWithParameter
                .If(_ => true, s => s.ExpectedUsage(vg, "IfBranch", 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1);

            Sut.FuncWithParameter(5);

            vg.Assert();
        }

        [Fact]
        public void ThrowWhenPassedNullAsNextStep()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MockMembers.FuncWithParameter.If(
                    v => true,
                    s => ((ICanHaveNextMethodStep<int, int>)s).SetNextStep((IMethodStep<int, int>)null!)
                )
            );
        }

        [Fact]
        public void CheckConditionInNoParameterCase()
        {
            bool flag = false;

            MockMembers.SimpleFunc
                // ReSharper disable once AccessToModifiedClosure
                .If(() => flag, s => s.Return(99))
                .Return(10);

            Assert.Equal(10, Sut.SimpleFunc());
            flag = true;
            Assert.Equal(99, Sut.SimpleFunc());
        }

        [Fact]
        public void CheckConditionInNoParameterOrReturnValueCase()
        {
            bool flag = false;

            var group = new VerificationGroup();

            MockMembers.SimpleAction
                // ReSharper disable once AccessToModifiedClosure
                .If(() => flag, s => s.ExpectedUsage(group, null, 1))
                .ExpectedUsage(group, null, 2);

            Sut.SimpleAction();
            Sut.SimpleAction();
            flag = true;
            Sut.SimpleAction();

            group.Assert();
        }

        [Fact]
        public void CheckConditionInNoReturnValueCase()
        {
            var group = new VerificationGroup();

            MockMembers.ActionWithParameter
                .If(a => a > 5, s => s.ExpectedUsage(group, null, 1))
                .ExpectedUsage(group, null, 2);

            Sut.ActionWithParameter(1);
            Sut.ActionWithParameter(6);
            Sut.ActionWithParameter(-3);

            group.Assert();
        }

        [Fact]
        public void UseElseBranchIfAskedInNoParameterCase()
        {
            var vg = new VerificationGroup();
            MockMembers.SimpleFunc
                .If(() => true, s => s.ExpectedUsage(vg, "IfBranch", 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1);

            Sut.SimpleFunc();

            vg.Assert();
        }

        [Fact]
        public void ThrowWhenPassedNullAsNextStepInNoParameterCase()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MockMembers.SimpleFunc.If(
                    () => true,
                    s => ((ICanHaveNextMethodStep<ValueTuple, int>)s).SetNextStep((IMethodStep<ValueTuple, int>)null!)
                )
            );
        }
    }
}
